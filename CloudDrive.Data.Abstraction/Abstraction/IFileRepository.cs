using CloudDrive.Application;

namespace CloudDrive.Data.Abstraction
{
    public interface IFileRepository
    {
        Task AddFile(AddUserFileVM file);
    }
}
