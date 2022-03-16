using CloudDrive.Application;

namespace CloudDrive.Data.Interfaces
{
    public interface IFileRepository
    {
        Task AddFile(AddUserFileVM file);
    }
}
