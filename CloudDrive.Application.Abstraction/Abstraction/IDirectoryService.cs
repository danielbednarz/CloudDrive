using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public interface IDirectoryService
    {
        Task<List<UserDirectoryDTO>> GetUserDriveDataToTreeView(string username);
        Task AddDirectory(AddDirectoryVM model, string username);
        Task<List<DirectorySelectBoxVM>> GetDirectoriesToSelectList(string username);
        //Task<DownloadDirectoryDTO> CreateCompressedDirectory(Guid id, string username);
        Task<DownloadDirectoryDTO> CreateCompressedDirectory(Guid id);
        Task<DownloadDirectoryDTO> CreateSelectedFilesDirectory(List<Guid> fileIds, string username);
    }
}
