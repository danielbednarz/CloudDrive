namespace CloudDrive.Domain
{
    public class UserFile
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public long Size { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
