using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public class FileDataDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public long FileVersion { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string RelativePath { get; set; }
        public Guid? DirectoryId { get; set; }
    }
}
