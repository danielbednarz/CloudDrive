using Microsoft.AspNetCore.Http;

namespace CloudDrive.Application
{
    public class DirectorySelectBoxVM
    {
        public string Text { get; set; }
        public Guid Value { get; set; }
        public Guid? ParentDirectoryId { get; set; }
    }
}
