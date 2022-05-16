using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public class UserDirectoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string RelativePath { get; set; }
        public bool IsFile { get; set; }
        public List<UserDirectoryDTO> Children { get; set; }
    }
}
