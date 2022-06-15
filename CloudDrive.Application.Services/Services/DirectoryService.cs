using CloudDrive.Application.Abstraction;
using CloudDrive.Data.Abstraction;
using CloudDrive.Domain;
using Microsoft.Extensions.Configuration;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace CloudDrive.Application
{
    public class DirectoryService : IDirectoryService
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public DirectoryService(IDirectoryRepository directoryRepository, IFileRepository fileRepository, IUserRepository userRepository, IConfiguration config)
        {
            _directoryRepository = directoryRepository;
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<List<UserItemDTO>> GetUserDriveDataToTreeView(string username, bool showDeleted)
        {
            List<UserDirectory> listDirectories = new();
            List<UserFile> listFiles = new();
            if (!showDeleted)
            {
                UserDirectory mainDirectory = _directoryRepository.FirstOrDefault(x => x.RelativePath == username) ?? throw new Exception("Uzytkownik nie posiada katalogu");
                listDirectories = await _directoryRepository.GetUserDriveDataToTreeView(username, mainDirectory.Id);
                listFiles = await _fileRepository.GetUserDriveFilesToTreeView(mainDirectory.Id);
            }
            else
            {
                UserDirectory archiveDirectory = _directoryRepository.FirstOrDefault(x => x.RelativePath == $"{username}\\archive") ?? throw new Exception("Uzytkownik nie posiada katalogu archiwum");
                //listDirectories = await _directoryRepository.GetUserDriveDeletedDataToTreeView(username, archiveDirectory.Id);
                listFiles = await _fileRepository.GetUserDriveDeletedFilesToTreeView(archiveDirectory.Id);
            }
            List<UserItemDTO> listDTO = FromUserDirectoryToDTO(listDirectories);
            listDTO.AddRange(FromUserFileToDTO(listFiles));

            return listDTO;
        }

        private List<UserItemDTO> FromUserDirectoryToDTO(List<UserDirectory> userDirectory)
        {
            return userDirectory.Select(x => new UserItemDTO()
            {
                Id = x.Id,
                Name = x.Name,
                RelativePath = x.RelativePath,
                Children = (x.ChildDirectories != null ? FromUserDirectoryToDTO(x.ChildDirectories.ToList()) : null).Concat(x.Files != null ? FromUserFileToDTO(x.Files.ToList()) : null).ToList(),
                Icon = "fa-solid fa-folder",
                IsFile = false,
                NoTick = true
            }).ToList();
        }

        private List<UserItemDTO> FromUserFileToDTO(List<UserFile> userFile)
        {
            var userFiles = userFile.GroupBy(x => x.RelativePath).Select(y => y.OrderByDescending(z => z.FileVersion).First()).Select(x => new UserItemDTO()
            {
                Id = x.Id,
                Name = x.Name,
                RelativePath = x.RelativePath,
                Icon = GetIconToFileType(x.ContentType),
                IsFile = true,
                NoTick = false
            }).ToList();

            return userFiles;
        }

        private static string GetIconToFileType(string contentType) =>
        contentType switch
        {
            "application/pdf" => "fa-solid fa-file-pdf",
            "image/png" or "image/jpeg" or "image/bmp" => "fa-solid fa-file-image",
            "video/avi" or "video/mp4" or "video/x-msvideo" => "fa-solid fa-file-video",
            "application/vnd.rar" or "application/x-zip-compressed" => "fa-solid fa-file-zipper",
            "application/msword" or "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "fa-solid fa-file-word",
            "audio/mpeg" => "fa-solid fa-file-audio",
            "text/plain" => "fa-solid fa-file-lines",
            _ => "fa-solid fa-file",
        };

        private string CreateDirectoryOnServer(string userChosenPath)
        {
            string mainPath = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>().SaveFilePath;
            string path = Path.Combine(mainPath, userChosenPath);

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return path;
        }

        public async Task AddDirectory(AddDirectoryVM model, string username)
        {
            if (!model.ParentDirectoryId.HasValue)
            {
                model.ParentDirectoryId = _directoryRepository.FirstOrDefault(x => x.RelativePath == username).Id;
            }

            UserDirectory parentDirectory = _directoryRepository.FirstOrDefault(x => x.Id == model.ParentDirectoryId) ?? throw new Exception("Wybrany folder nadrzedny nie istnieje");
            model.GeneratedPath = Path.Combine(parentDirectory.RelativePath, model.Name);

            AppUser user = _userRepository.FirstOrDefault(x => x.Username == username) ?? throw new Exception("Użytkownik nie istnieje");
            model.UserId = user.Id;

            if (_directoryRepository.IsDirectoryUnique(model.GeneratedPath))
            {
                CreateDirectoryOnServer(model.GeneratedPath);
                await _directoryRepository.AddDirectory(model);
            }
            else
            {
                return;
                //throw new Exception("Folder o podanej ścieżce już istnieje");
            }
        }

        public async Task<List<DirectorySelectBoxVM>> GetDirectoriesToSelectList(string username)
        {
            AppUser user = _userRepository.FirstOrDefault(x => x.Username == username) ?? throw new Exception("Użytkownik nie istnieje");
            return await _directoryRepository.GetDirectoriesToSelectList(user.Id, username);
        }

        public async Task<DownloadDirectoryDTO> CreateCompressedDirectory(Guid id)
        {
            string mainPath = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>().SaveFilePath;

            UserDirectory directory = await _directoryRepository.GetDirectoryById(id);

            string directoryPath = Path.Combine(mainPath, directory.RelativePath);
            string compressedDirectoryPath = directoryPath + ".zip";

            if (File.Exists(compressedDirectoryPath))   // usunięcie zipa jeśli już istnieje o identycznej nazwie
            {
                File.Delete(compressedDirectoryPath);
            }

            ZipFile.CreateFromDirectory(directoryPath, compressedDirectoryPath);
            await RenameZipFiles(compressedDirectoryPath, directory.RelativePath);

            try
            {
                byte[] fileContent = File.ReadAllBytes(compressedDirectoryPath);

                return new DownloadDirectoryDTO { Bytes = fileContent, DirectoryName = directory.Name + ".zip" };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task RenameZipFiles(string zipDirectory, string directoryRelativePath)
        {
            using var archive = new ZipArchive(File.Open(zipDirectory, FileMode.Open, FileAccess.ReadWrite), ZipArchiveMode.Update);

            var files = archive.Entries.ToArray();
            foreach (var entry in files)
            {
                var fileId = Guid.Parse(entry.Name);

                UserFile file = await _fileRepository.GetFileByIdAsync(fileId);

                var fileNameWithSubdirectory = file.RelativePath.Replace(directoryRelativePath + "\\", "");

                var newZipFileEntry = archive.CreateEntry(fileNameWithSubdirectory);
                using (var zipFile = entry.Open())
                {
                    using var newZipFile = newZipFileEntry.Open();
                    zipFile.CopyTo(newZipFile);

                }

                entry.Delete();
            }
        }

        public async Task<DownloadDirectoryDTO> CreateSelectedFilesDirectory(List<Guid> fileIds, string username)
        {
            string mainPath = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>().SaveFilePath;

            UserDirectory directory = await _directoryRepository.GetDirectoryByRelativePath(username, username);    // pobranie głównego katalogu danego użytkownika

            string directoryPath = Path.Combine(mainPath, directory.RelativePath);
            string compressedDirectoryPath = directoryPath + ".zip";

            using FileStream zipFile = File.Open(compressedDirectoryPath, FileMode.Create);

            await AddSelectedFilesToDirectory(zipFile, fileIds, mainPath);

            try
            {
                byte[] fileContent = File.ReadAllBytes(compressedDirectoryPath);

                return new DownloadDirectoryDTO { Bytes = fileContent, DirectoryName = "Selected_files.zip" };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task AddSelectedFilesToDirectory(FileStream zipFile, List<Guid> fileIds, string mainPath)
        {
            using var archive = new ZipArchive(zipFile, ZipArchiveMode.Create);

            foreach (var fileId in fileIds)
            {
                UserFile file = await _fileRepository.GetFileByIdAsync(fileId);

                var absolutePathToFileWithFileName = Path.Combine(mainPath, file.RelativePath);
                var absolutePathToFileWithId = absolutePathToFileWithFileName.Replace(file.Name, file.Id.ToString());   //nazwa pliku z Guid -> FileName

                archive.CreateEntryFromFile(absolutePathToFileWithId, file.Name);
            }

            archive.Dispose();  // zwolnienie obiektu, by nie doprowadzić do deadlock
        }
    }
}
