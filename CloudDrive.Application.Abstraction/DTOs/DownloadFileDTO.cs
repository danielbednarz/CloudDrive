using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public class DownloadFileDTO
    {
        public byte[] Bytes { get; set; }
        public string Path { get; set; }
        public UserFile UserFile { get; set; }
    }
}
