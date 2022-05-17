namespace CloudDrive.Domain
{
    public class UserDirectory
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public Guid? ParentDirectoryId { get; set; }
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual AppUser User { get; set; }
        public virtual UserDirectory ParentDirectory { get; set; }
        public virtual ICollection<UserFile> Files { get; set; }
        public virtual ICollection<UserDirectory> ChildDirectories { get; set; }
    }
}
