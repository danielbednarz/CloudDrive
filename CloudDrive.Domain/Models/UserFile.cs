﻿namespace CloudDrive.Domain
{
    public class UserFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public long FileVersion { get; set; }
        public string ContentType { get;set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string RelativePath { get; set; }
        public int UserId { get; set; }
        public Guid? DirectoryId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual AppUser User { get; set; }
        public virtual UserDirectory Directory { get; set; }
        public virtual ICollection<FileOperationsLogs> FileOperationsLogs { get; set; }
    }
}
