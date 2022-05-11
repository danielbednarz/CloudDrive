namespace CloudDrive.Application
{
    public class AddDirectoryVM
    {
        public string Name { get; set; }
        public string GeneratedPath { get; set; }
        public int? UserId { get; set; }
        public Guid? ParentDirectoryId { get; set; }
    }
}
