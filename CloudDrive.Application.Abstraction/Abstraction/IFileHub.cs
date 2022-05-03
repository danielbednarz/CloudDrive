namespace CloudDrive.Application
{
    public interface IFileHub
    {
        Task FileAdded(Guid id, string fileName);
    }
}
