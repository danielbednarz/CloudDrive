using CloudDrive.Application;
using CloudDrive.Data.Abstraction;
using CloudDrive.Domain;
using CloudDrive.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CloudDrive.Data.Repositories
{
    public class FileRepository : GenericRepository<UserFile>, IFileRepository
    {
        public FileRepository(MainDatabaseContext context) : base(context)
        {
        }

        public async Task<List<FileDataDTO>> GetUserFiles(int userId)
        {
            return await _context.Files.Where(x => x.UserId == userId && x.IsDeleted == false).Select(x => new FileDataDTO
            {
                Id = x.Id,
                Name = x.Name,
                Size = x.Size,
                FileVersion = x.FileVersion,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
                RelativePath = x.RelativePath,
                DirectoryId = x.DirectoryId
            }).ToListAsync();
        }
        
        public async Task<UserFile> AddFile(AddUserFileVM file, UserDirectory userDirectory)
        {
            var fileName = file.File.FileName;
            long fileVersion = 0;

            var fileContentType = GetMimeType(file.File.FileName);

            if (_context.Files.Any(x => x.Name == fileName))
            {
                fileVersion = _context.Files.Where(y => y.Name == fileName && y.ContentType == fileContentType)
                    .Select(x => x.FileVersion).ToList().Max();

                fileVersion += 1;
            }

            UserFile userFile = new()
            {
                Name = fileName,
                Size = file.File.Length,
                FileVersion = fileVersion,
                CreatedDate = DateTime.Now,
                ContentType = fileContentType,
                RelativePath = @$"{userDirectory.RelativePath}\{fileName}",
                UserId = file.UserId.Value,
                DirectoryId = userDirectory.Id
            };

            await _context.Files.AddAsync(userFile);
            await _context.SaveChangesAsync();

            return userFile;
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();

            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }

            return mimeType;
        }

        public async Task<UserFile> GetFileById(Guid fileId)
        {
            return await _context.Files.FirstOrDefaultAsync(x => x.Id == fileId);
        }

        public async Task<UserFile> MarkFileAsDeleted(string filePath, int? userId)
        {
            var file = await _context.Files
                .Where(x => x.RelativePath == filePath && x.User.Id == userId && x.IsDeleted == false)
                .OrderByDescending(x => x.FileVersion)
                .FirstOrDefaultAsync();

            file.IsDeleted = true;
            file.RelativePath = @$"archive\{file.Name}";

            _context.Files.Update(file);
            await _context.SaveChangesAsync();

            return file;
        }

    }
}
