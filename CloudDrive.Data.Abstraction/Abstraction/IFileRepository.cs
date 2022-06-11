using CloudDrive.Application;
using CloudDrive.Domain;

namespace CloudDrive.Data.Abstraction
{
    public interface IFileRepository : IGenericRepository<UserFile>
    {
        Task<List<FileDataDTO>> GetUserFiles(int userId);
        Task<UserFile> AddFile(AddUserFileVM file, UserDirectory userDirectory);
        Task<UserFile> GetFileByIdAsync(Guid fileId);
        UserFile GetFileById(Guid fileId);
        Task<List<UserFile>> GetAllFileVersions(UserFile file);
        Task<UserFile> MarkFileAsDeleted(string filePath, int? userId, string username);
        Task MarkFileAsCurrentById(Guid fileId, int? userId, string currentRelativePath, Guid? currentDirectoryId);
        Task<List<UserFile>> GetUserDriveFilesToTreeView(Guid mainDirectoryId);
        string GetMimeType(string fileName);
    }
}
