using CloudDrive.Application.Abstraction;
using CloudDrive.Data.Abstraction;
using CloudDrive.Domain;
using Microsoft.Extensions.Configuration;

namespace CloudDrive.Application
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IConfiguration _config;

        public FileService(IFileRepository fileRepository, IUserRepository userRepository, IConfiguration config, IDirectoryRepository directoryRepository)
        {
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _config = config;
            _directoryRepository = directoryRepository;
        }

        public async Task<UserFile> AddFile(AddUserFileVM file)
        {
            var fileUploadConfig = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>();

            file.UserId = _userRepository.FirstOrDefault(x => x.Username == file.Username)?.Id;

            UserDirectory userDirectory = _directoryRepository.FirstOrDefault(x => x.UserId == file.UserId && x.ParentDirectoryId == null);
            UserFile userFile = await _fileRepository.AddFile(file, userDirectory);

            using var stream = File.Create($"{fileUploadConfig.SaveFilePath}\\{file.Username}\\{userFile.Id.ToString()}");
            await file.File.CopyToAsync(stream);

            return userFile;
        }

        public async Task DeleteFile(string relativePath, string username)
        {
            var fileUploadConfig = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>();

            var userId = _userRepository.FirstOrDefault(x => x.Username == username)?.Id;

            UserFile file = await _fileRepository.MarkFileAsDeleted(relativePath, userId);

            string filePath = $"{fileUploadConfig.SaveFilePath}\\{relativePath.Replace(file.Name, file.Id.ToString())}";
            string filePath2 = $"{fileUploadConfig.SaveFilePath}\\{username}\\archive\\{file.Id}";

            File.Move(filePath, filePath2);
        }

        public async Task<DownloadFileDTO> DownloadFile(Guid fileId, string username)
        {
            var fileUploadConfig = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>();
            var file = await _fileRepository.GetFileById(fileId);

            if (file == null)
            {
                return null;
            }

            string filePath = $"{fileUploadConfig.SaveFilePath}\\{username}\\{file.Id.ToString()}";

            var bytes = await File.ReadAllBytesAsync(filePath);

            return new DownloadFileDTO { Bytes = bytes, Path = filePath, UserFile = file };
        }
    }
}
