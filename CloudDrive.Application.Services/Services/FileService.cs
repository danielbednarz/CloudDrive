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
        private readonly IConfiguration _config;

        public FileService(IFileRepository fileRepository, IUserRepository userRepository, IConfiguration config)
        {
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<UserFile> AddFile(AddUserFileVM file)
        {
            var fileUploadConfig = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>();

            file.UserId = _userRepository.FirstOrDefault(x => x.Username == file.Username)?.Id;

            UserFile userFile = await _fileRepository.AddFile(file);
            
            var fileName = userFile.Id.ToString();

            using var stream = File.Create($"{fileUploadConfig.SaveFilePath}\\{file.Username}\\{fileName}");
            await file.File.CopyToAsync(stream);

            return userFile;
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
