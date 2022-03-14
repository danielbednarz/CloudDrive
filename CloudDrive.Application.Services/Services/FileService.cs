using CloudDrive.Data.Interfaces;

namespace CloudDrive.Application
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
    
        public void AddFile()
        {
            _fileRepository.AddFile();
        }
    }
}
