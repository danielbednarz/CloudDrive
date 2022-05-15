using Microsoft.AspNetCore.Http;

namespace CloudDrive.Application
{
    public class FileAddVM
    {
        public Guid? DirectoryId { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}
