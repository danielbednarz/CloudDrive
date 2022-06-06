using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public class DownloadDirectoryDTO
    {
        public byte[] Bytes { get; set; }
        public string DirectoryName { get; set; }
    }
}
