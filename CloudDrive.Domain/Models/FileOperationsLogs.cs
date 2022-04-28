using CloudDrive.Dictionaries;

namespace CloudDrive.Domain
{
    public class FileOperationsLogs
    {
        public Guid Id { get; set; }
        public OperationType OperationType { get; set; }
        public DateTime Date { get; set; }
        public Guid FileId { get; set; }
        public virtual UserFile File { get; set; } 
    }
}
