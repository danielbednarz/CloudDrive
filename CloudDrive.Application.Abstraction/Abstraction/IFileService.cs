using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public interface IFileService
    {
        Task<UserFile> AddFile(AddUserFileVM file);
        Task<DownloadFileDTO> DownloadFile(Guid fileId, string username);
        Task DeleteFile(string relativePath, string username);
    }
}
