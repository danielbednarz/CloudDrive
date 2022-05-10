using CloudDrive.Application;
using CloudDrive.Domain;

namespace CloudDrive.Data.Abstraction
{
    public interface IDirectoryRepository
    {
        Task AddDirectory(AddDirectoryVM model);
        bool IsDirectoryUnique(string path);
    }
}
