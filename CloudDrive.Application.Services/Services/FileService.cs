using CloudDrive.Application.Abstraction;
using CloudDrive.Data.Abstraction;
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

        public async Task AddFile(AddUserFileVM file)
        {
            var fileName = file.File.FileName;

            var fileUploadConfig = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>();

            file.UserId = _userRepository.FirstOrDefault(x => x.Username == file.Username)?.Id;

            using var stream = File.Create($"{fileUploadConfig.SaveFilePath}\\{file.Username}\\{fileName}");
            await file.File.CopyToAsync(stream);

            await _fileRepository.AddFile(file);
        }
    }
}
