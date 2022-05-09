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

        public async Task<UserFile> AddFile(AddUserFileVM file)
        {
            var fileName = file.File.FileName;
            long fileVersion = 0;

            if (_context.Files.Any(x => x.Name == fileName))
            {
                fileVersion = _context.Files.Where(y => y.Name == fileName)
                    .Select(x => x.FileVersion).ToList().Max();

                fileVersion += 1;
            }

            UserFile userFile = new UserFile()
            {
                Name = fileName,
                Size = file.File.Length,
                FileVersion = fileVersion,
                CreatedDate = DateTime.Now,
                ContentType = file.File.ContentType,
                UserId = file.UserId.Value
            };

            await _context.Files.AddAsync(userFile);
            await _context.SaveChangesAsync();

            return userFile;
        }

        public async Task<UserFile> GetFileById(Guid fileId)
        {
            return await _context.Files.FirstOrDefaultAsync(x => x.Id == fileId);
        }
    }
}
