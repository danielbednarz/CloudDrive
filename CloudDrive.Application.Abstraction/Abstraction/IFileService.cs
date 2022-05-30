using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public interface IFileService
    {
        Task<List<FileDataDTO>> GetUserFiles(string username);
        Task<UserFile> AddFileByFileWatcher(AddUserFileVM file, string relativePath);
        Task<UserFile> AddFile(AddUserFileVM file);
        Task<DownloadFileDTO> DownloadFile(Guid fileId, string username);
        Task<List<FileVersionDTO>> GetFileVersions(Guid fileId);
        Task DeleteFile(string relativePath, string username);
        Task SelectFileVersion(Guid fileId, string username);
    }
}
