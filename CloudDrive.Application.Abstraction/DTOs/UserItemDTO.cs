using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public class UserItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string RelativePath { get; set; }
        public bool IsFile { get; set; }
        public bool NoTick { get; set; }
        public List<UserItemDTO> Children { get; set; }
    }
}
