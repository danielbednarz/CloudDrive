using Microsoft.AspNetCore.Http;

namespace CloudDrive.Application
{
    public class AddUserFileVM
    {
        public IFormFile File { get; set; }
        public string Username { get; set; }
        public int? UserId { get; set; }
    }
}
