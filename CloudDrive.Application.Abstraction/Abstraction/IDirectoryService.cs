using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public interface IDirectoryService
    {
        Task AddDirectory(AddDirectoryVM model, string username);
        Task<List<DirectorySelectBoxVM>> GetDirectoriesToSelectList(string username);
    }
}
