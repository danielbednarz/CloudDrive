using CloudDrive.Application.Abstraction;
using CloudDrive.Data.Abstraction;
using CloudDrive.Domain;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace CloudDrive.Application
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IConfiguration _config;
        private readonly IDirectoryService _directoryService;

        public FileService(IFileRepository fileRepository, IUserRepository userRepository, IConfiguration config, IDirectoryRepository directoryRepository, IDirectoryService directoryService)
        {
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _config = config;
            _directoryRepository = directoryRepository;
            _directoryService = directoryService;
        }

        public async Task<List<FileDataDTO>> GetUserFiles(string username)
        {
            AppUser user = _userRepository.FirstOrDefault(x => x.Username == username) ?? throw new Exception("Nie znaleziono uzytkownika");
            return await _fileRepository.GetUserFiles(user.Id);
        }

        public async Task<UserFile> AddFile(AddUserFileVM file)
        {
            var fileUploadConfig = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>();

            file.UserId = _userRepository.FirstOrDefault(x => x.Username == file.Username)?.Id;

            if (!file.DirectoryId.HasValue)
            {
                file.DirectoryId = _directoryRepository.FirstOrDefault(x => x.RelativePath == file.Username).Id;
            }

            UserDirectory userDirectory = _directoryRepository.FirstOrDefault(x => x.Id == file.DirectoryId);
            UserFile userFile = await _fileRepository.AddFile(file, userDirectory);

            using var stream = File.Create($"{fileUploadConfig.SaveFilePath}\\{userDirectory.RelativePath}\\{userFile.Id.ToString()}");
            await file.File.CopyToAsync(stream);

            return userFile;
        }

        public async Task<UserFile> AddFileByFileWatcher(AddUserFileVM file, string relativePath)
        {
            var fileUploadConfig = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>();

            file.UserId = _userRepository.FirstOrDefault(x => x.Username == file.Username)?.Id;

            var relativePathWithFileName = relativePath;

            relativePath = file.Username + relativePath.TrimEnd('\\').Remove(relativePath.LastIndexOf('\\') + 1);   //usunięcie nazwy pliku ze ścieżki

            UserFile userFile = new();

            // w przypadku, gdy wrzucamy do głównego katalogu, który jest nadzorowany
            var mainDirectoryPath = file.Username + "\\";
            if (relativePath == mainDirectoryPath)
            {
                file.DirectoryId = _directoryRepository.FirstOrDefault(x => x.RelativePath == file.Username).Id;

                var mainDirectory = _directoryRepository.FirstOrDefault(x => x.Id == file.DirectoryId);
                userFile = await _fileRepository.AddFile(file, mainDirectory);

                using var stream = File.Create($"{fileUploadConfig.SaveFilePath}\\{mainDirectory.RelativePath}\\{userFile.Id.ToString()}");
                await file.File.CopyToAsync(stream);

                return userFile;
            }

            // pobranie katalogu do którego należy dodać plik
            var userDirectory = await _directoryRepository.FirstOrDefaultAsync(x => x.RelativePath == relativePath);

            // w przypadku, gdy katalogu nie ma na serwerze
            if (userDirectory == null)
            {
                var lastFolder = Path.GetDirectoryName(relativePath);
                var parentPath = Path.GetDirectoryName(lastFolder);

                UserDirectory parentDirectory = _directoryRepository.FirstOrDefault(x => x.RelativePath == parentPath);

                AddDirectoryVM addDirectoryVM = new()
                {
                    Name = relativePath.Split('\\').SkipLast(1).Last(), //wycięcie ze scieżki nazwy pliku
                    GeneratedPath = relativePath,
                    UserId = file.UserId,
                    ParentDirectoryId = parentDirectory.Id
                };

                await _directoryService.AddDirectory(addDirectoryVM, file.Username);
                userDirectory = _directoryRepository.FirstOrDefault(x => x.RelativePath == lastFolder);
            }

            userFile = await _fileRepository.AddFile(file, userDirectory);
            userFile.RelativePath = relativePathWithFileName;

            using var stream2 = File.Create($"{fileUploadConfig.SaveFilePath}\\{relativePath}{userFile.Id.ToString()}");
            await file.File.CopyToAsync(stream2);

            return userFile;
        }

        public async Task DeleteFile(string relativePath, string username)
        {
            var fileUploadConfig = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>();

            var userId = _userRepository.FirstOrDefault(x => x.Username == username)?.Id;

            UserFile file = await _fileRepository.MarkFileAsDeleted(relativePath, userId, username);

            string oldPath = $"{fileUploadConfig.SaveFilePath}\\{relativePath.Replace(file.Name, file.Id.ToString())}";
            string newPath = $"{fileUploadConfig.SaveFilePath}\\{username}\\archive\\{file.Id}";

            File.Move(oldPath, newPath);
        }

        public async Task<DownloadFileDTO> DownloadFile(Guid fileId, string username)
        {
            var fileUploadConfig = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>();
            var file = await _fileRepository.GetFileByIdAsync(fileId);
            var fileDirectory = await _directoryRepository.FirstOrDefaultAsync(x => x.User.Username == username && x.Id == file.DirectoryId);

            if (file == null)
            {
                return null;
            }

            string filePath = $"{fileUploadConfig.SaveFilePath}\\{fileDirectory.RelativePath}\\{file.Id.ToString()}";

            var bytes = await File.ReadAllBytesAsync(filePath);

            return new DownloadFileDTO { Bytes = bytes, Path = filePath, UserFile = file };
        }

        public async Task<List<FileVersionDTO>> GetFileVersions(Guid fileId)
        {
            UserFile file = await _fileRepository.GetFileByIdAsync(fileId);
            List<UserFile> allVersions = await _fileRepository.GetAllFileVersions(file);

            return allVersions.Select(x => new FileVersionDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Size = x.Size,
                FileVersion = x.FileVersion,
                CreatedDate = x.CreatedDate,
                IsDeleted = x.IsDeleted
            }).ToList();
        }

        public async Task SelectFileVersion(Guid fileId, string username)
        {
            var fileUploadConfig = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>();

            var userId = _userRepository.FirstOrDefault(x => x.Username == username)?.Id;

            UserFile newVersion = _fileRepository.FirstOrDefault(x => x.Id == fileId);
            UserFile currentVersion = _fileRepository.FirstOrDefault(x => x.Name == newVersion.Name && x.ContentType == newVersion.ContentType && !x.IsDeleted);
            string currentRelativePath;
            Guid? currentDirectoryId;

            if (currentVersion != null)
            {
                currentRelativePath = currentVersion.RelativePath;
                currentDirectoryId = currentVersion.DirectoryId;
                await _fileRepository.MarkFileAsDeleted(currentRelativePath, userId, username);
                string oldPathCurrent = $"{fileUploadConfig.SaveFilePath}\\{currentRelativePath.Replace(currentVersion.Name, currentVersion.Id.ToString())}";
                string newPathCurrent = $"{fileUploadConfig.SaveFilePath}\\{username}\\archive\\{currentVersion.Id}";
                File.Move(oldPathCurrent, newPathCurrent);
            }
            else
            {
                UserDirectory directory = _directoryRepository.FirstOrDefault(x => x.RelativePath == username) ?? throw new Exception("Brak folderu uzytkownika");
                currentRelativePath = $"{directory.RelativePath}\\{newVersion.Name}";
                currentDirectoryId = directory.Id;
            }

            await _fileRepository.MarkFileAsCurrentById(fileId, userId, currentRelativePath, currentDirectoryId);

            string oldPath = $"{fileUploadConfig.SaveFilePath}\\{username}\\archive\\{fileId}";
            string newPath = $"{fileUploadConfig.SaveFilePath}\\{currentRelativePath.Replace(newVersion.Name, fileId.ToString())}";
            File.Move(oldPath, newPath);
        }
    }
}
