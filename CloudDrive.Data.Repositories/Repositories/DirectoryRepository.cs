using CloudDrive.Application;
using CloudDrive.Data.Abstraction;
using CloudDrive.Domain;
using CloudDrive.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CloudDrive.Data.Repositories
{
    public class DirectoryRepository : GenericRepository<UserDirectory>, IDirectoryRepository
    {
        public DirectoryRepository(MainDatabaseContext context) : base(context)
        {
        }

        public async Task AddDirectory(AddDirectoryVM model)
        {
            await _context.UserDirectories.AddAsync(new UserDirectory
            {
                Name = model.Name,
                RelativePath = model.UserChosenPath,
                UserId = model.UserId.Value
            });

            _context.SaveChanges();
        }

        public bool IsDirectoryUnique(string path)
        {
           return !_context.UserDirectories.Any(x => x.RelativePath == path);
        }
    }
}
