using CloudDrive.Application;
using CloudDrive.Domain;

namespace CloudDrive.Data.Abstraction
{
    public interface IDirectoryRepository : IGenericRepository<UserDirectory>
    {
        Task<List<UserDirectory>> GetUserDriveDataToTreeView(string username, Guid mainDirectoryId);
        Task AddDirectory(AddDirectoryVM model);
        bool IsDirectoryUnique(string path);
        Task<List<DirectorySelectBoxVM>> GetDirectoriesToSelectList(int userId, string username);
    }
}
