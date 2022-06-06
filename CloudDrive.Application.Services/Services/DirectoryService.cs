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

        public async Task<List<UserDirectoryDTO>> GetUserDriveDataToTreeView(string username)
        {
            UserDirectory mainDirectory = _directoryRepository.FirstOrDefault(x => x.RelativePath == username) ?? throw new Exception("Uzytkownik nie posiada katalogu");
            List<UserDirectory> listDirectories = await _directoryRepository.GetUserDriveDataToTreeView(username, mainDirectory.Id);
            List<UserFile> listFiles = await _fileRepository.GetUserDriveFilesToTreeView(mainDirectory.Id);
            List<UserDirectoryDTO> listDTO = FromUserDirectoryToDTO(listDirectories);
            listDTO.AddRange(FromUserFileToDTO(listFiles));

            return listDTO;
        }

        private List<UserDirectoryDTO> FromUserDirectoryToDTO(List<UserDirectory> userDirectory)
        {
            return userDirectory.Select(x => new UserDirectoryDTO()
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

        private List<UserDirectoryDTO> FromUserFileToDTO(List<UserFile> userFile)
        {
            var userFiles = userFile.Where(x => !x.IsDeleted).GroupBy(x => x.RelativePath).Select(y => y.OrderByDescending(z => z.FileVersion).First()).Select(x => new UserDirectoryDTO()
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

        public async Task<DownloadDirectoryDTO> CreateCompressedDirectory(Guid id, string username)
        {
            string mainPath = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>().SaveFilePath;

            UserDirectory directory = await _directoryRepository.GetDirectoryById(id);

            string directoryPath = Path.Combine(mainPath, directory.RelativePath);
            string compressedDirectoryPath = directoryPath + ".zip";

            using FileStream zipFile = File.Open(compressedDirectoryPath, FileMode.Create);

            await AddFilesToCompressedDirectory(id, mainPath, zipFile);

            string[] childDirectoriesPaths = GetChildDirectoriesPaths(directoryPath);
            foreach (var childDirectoryPath in childDirectoriesPaths)
            {
                string relativeChildDirectoryPath = childDirectoryPath.Substring(mainPath.Length + 1);  // stworzenie ścieżki względnej

                UserDirectory subDirectory = await _directoryRepository.GetDirectoryByRelativePath(relativeChildDirectoryPath, username);

                await AddFilesToParentCompressedDirectory(subDirectory, mainPath, compressedDirectoryPath);
            }

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

        private async Task AddFilesToCompressedDirectory(Guid directoryId, string mainPath, FileStream zipFile)
        {
            using var archive = new ZipArchive(zipFile, ZipArchiveMode.Create);

            var filesFromDirectory = await _directoryRepository.GetFilesFromDirectory(directoryId);
            foreach (var file in filesFromDirectory)
            {
                var absolutePathToFileWithFileName = Path.Combine(mainPath, file.RelativePath);
                var absolutePathToFileWithId = absolutePathToFileWithFileName.Replace(file.Name, file.Id.ToString());   //nazwa pliku z Guid -> FileName

                archive.CreateEntryFromFile(absolutePathToFileWithId, file.Name);
            }

            archive.Dispose();
        }

        private async Task AddFilesToParentCompressedDirectory(UserDirectory directory, string mainPath, string compressedDirectoryPath)
        {
            using FileStream zipFile = File.Open(compressedDirectoryPath, FileMode.OpenOrCreate);
            using var archive = new ZipArchive(zipFile, ZipArchiveMode.Update);

            var filesFromDirectory = await _directoryRepository.GetFilesFromDirectory(directory.Id);
            foreach (var file in filesFromDirectory)
            {
                var absolutePathToFileWithFileName = Path.Combine(mainPath, file.RelativePath);
                var absolutePathToFileWithId = absolutePathToFileWithFileName.Replace(file.Name, file.Id.ToString());   //nazwa pliku z Guid -> FileName

                archive.CreateEntryFromFile(absolutePathToFileWithId, file.Name);
            }

            archive.Dispose();
        }

        private static string[] GetChildDirectoriesPaths(string directoryPath)
        {
            return Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories);
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
                UserFile file = await _fileRepository.GetFileById(fileId);

                var absolutePathToFileWithFileName = Path.Combine(mainPath, file.RelativePath);
                var absolutePathToFileWithId = absolutePathToFileWithFileName.Replace(file.Name, file.Id.ToString());   //nazwa pliku z Guid -> FileName

                archive.CreateEntryFromFile(absolutePathToFileWithId, file.Name);
            }

            archive.Dispose();  // zwolnienie obiektu, by nie doprowadzić do deadlock
        }
    }
}
