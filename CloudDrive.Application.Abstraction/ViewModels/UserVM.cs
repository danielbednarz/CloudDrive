using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Application
{
    public class UserVM
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
