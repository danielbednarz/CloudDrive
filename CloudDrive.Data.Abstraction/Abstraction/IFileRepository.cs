using CloudDrive.Application;
using CloudDrive.Domain;

namespace CloudDrive.Data.Abstraction
{
    public interface IFileRepository
    {
        Task<UserFile> AddFile(AddUserFileVM file);
        Task<UserFile> GetFileById(Guid fileId);

    }
}
