using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public class FileVersionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public long FileVersion { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
