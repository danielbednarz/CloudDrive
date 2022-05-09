namespace CloudDrive.Domain
{
    public class DownloadFile
    {
        public byte[] Bytes { get; set; }
        public string Path { get; set; }
        public UserFile UserFile { get; set; }
    }
}
