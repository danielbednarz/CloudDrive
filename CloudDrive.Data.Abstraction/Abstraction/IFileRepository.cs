using CloudDrive.Application;
using CloudDrive.Domain;

namespace CloudDrive.Data.Abstraction
{
    public interface IFileRepository : IGenericRepository<UserFile>
    {
        Task<List<FileDataDTO>> GetUserFiles(int userId);
        Task<UserFile> AddFile(AddUserFileVM file, UserDirectory userDirectory);
        Task<UserFile> GetFileById(Guid fileId);
        Task<UserFile> MarkFileAsDeleted(string filePath, int? userId);
        Task<List<UserFile>> GetUserDriveFilesToTreeView(Guid mainDirectoryId);
    }
}
