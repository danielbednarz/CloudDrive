namespace CloudDrive.Application
{
    public interface IFileService
    {
        Task AddFile(AddUserFileVM file);
    }
}
