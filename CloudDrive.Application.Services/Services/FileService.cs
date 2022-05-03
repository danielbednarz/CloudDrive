using CloudDrive.Application.Abstraction;
using CloudDrive.Data.Abstraction;
using CloudDrive.Domain;
using Microsoft.Extensions.Configuration;

namespace CloudDrive.Application
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IConfiguration _config;

        public FileService(IFileRepository fileRepository, IConfiguration config)
        {
            _fileRepository = fileRepository;
            _config = config;
        }

        public async Task<UserFile> AddFile(AddUserFileVM file)
        {
            var fileName = file.File.FileName;

            var fileUploadConfig = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>();

            using (var stream = File.Create($"{fileUploadConfig.SaveFilePath}\\{fileName}"))
            {
                await file.File.CopyToAsync(stream);
            }

            UserFile userFile = await _fileRepository.AddFile(file);

            return userFile;
        }
    }
}
