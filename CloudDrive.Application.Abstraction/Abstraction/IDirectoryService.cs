using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public interface IDirectoryService
    {
        Task<List<UserDirectoryDTO>> GetUserDriveDataToTreeView(string username);
        Task AddDirectory(AddDirectoryVM model, string username);
        Task<List<DirectorySelectBoxVM>> GetDirectoriesToSelectList(string username);
    }
}
