using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public interface IFileService
    {
        Task<UserFile> AddFile(AddUserFileVM file);
    }
}
