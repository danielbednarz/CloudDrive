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
                IsFile = false
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
                IsFile = true
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

            UserDirectory downloadDirectory = await _directoryRepository.GetDirectoryById(id);

            string downloadDirectoryPath = Path.Combine(mainPath, downloadDirectory.RelativePath);
            string downloadCompressedDirectoryPath = downloadDirectoryPath + ".zip";

            using FileStream zipFile = File.Open(downloadCompressedDirectoryPath, FileMode.Create);
             
            await AddFilesToDownloadCompressedDirectory(id, mainPath, zipFile);

            string[] childDirectoriesPaths = GetChildDirectoriesPaths(downloadDirectoryPath);
            foreach (var childDirectoryPath in childDirectoriesPaths)
            {
                string relativeChildDirectoryPath = childDirectoryPath.Substring(mainPath.Length + 1);  // stworzenie ścieżki względnej

                UserDirectory subDirectory = await _directoryRepository.GetDirectoryByRelativePath(relativeChildDirectoryPath, username);

                await AddFilesToParentCompressedDirectory(subDirectory.Id, mainPath, downloadCompressedDirectoryPath);
            }

            try
            { 
                byte[] fileContent = File.ReadAllBytes(downloadCompressedDirectoryPath);

                return new DownloadDirectoryDTO { Bytes = fileContent, DirectoryName = downloadDirectory.Name + ".zip" };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task AddFilesToDownloadCompressedDirectory(Guid directoryId, string mainPath, FileStream zipFile)
        {
            var filesFromDirectory = await _directoryRepository.GetFilesFromDirectory(directoryId);

            using var archive = new ZipArchive(zipFile, ZipArchiveMode.Create);
            foreach (var file in filesFromDirectory)
            {
                var absolutePathToFileWithFileName = Path.Combine(mainPath, file.RelativePath);
                var absolutePathToFileWithId = absolutePathToFileWithFileName.Replace(file.Name, file.Id.ToString());   //nazwa pliku z Guid -> FileName

                archive.CreateEntryFromFile(absolutePathToFileWithId, file.Name);
            }

            archive.Dispose();
        }

        private async Task AddFilesToParentCompressedDirectory(Guid directoryId, string mainPath, string downloadCompressedDirectoryPath)
        {
            var filesFromDirectory = await _directoryRepository.GetFilesFromDirectory(directoryId);

            using FileStream zipFile = File.Open(downloadCompressedDirectoryPath, FileMode.OpenOrCreate);
            using var archive = new ZipArchive(zipFile, ZipArchiveMode.Update);
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

    }
}
