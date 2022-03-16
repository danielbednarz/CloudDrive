using Microsoft.AspNetCore.Http;

namespace CloudDrive.Application
{
    public class AddUserFileVM
    {
        public IFormFile File { get; set; }
        public long FileVersion { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
