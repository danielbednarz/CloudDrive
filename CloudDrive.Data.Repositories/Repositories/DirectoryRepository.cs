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

        public async Task<List<UserDirectory>> GetUserDriveDataToTreeView(string username, Guid mainDirectoryId)
        {
            return await _context.UserDirectories
                .OrderBy(x => x.CreatedDate)
                .Where(x => x.ParentDirectoryId == mainDirectoryId && x.RelativePath != username && x.RelativePath != $"{username}\\archive")
                .ToListAsync();
        }

        public async Task AddDirectory(AddDirectoryVM model)
        {
            await _context.UserDirectories.AddAsync(new UserDirectory
            {
                Name = model.Name,
                RelativePath = model.GeneratedPath,
                UserId = model.UserId.Value,
                ParentDirectoryId = model.ParentDirectoryId,
                CreatedDate = DateTime.Now
            });

            _context.SaveChanges();
        }

        public bool IsDirectoryUnique(string path)
        {
           return !_context.UserDirectories.Any(x => x.RelativePath == path);
        }

        public async Task<List<DirectorySelectBoxVM>> GetDirectoriesToSelectList(int userId, string username)
        {
           return await _context.UserDirectories.Where(x => x.RelativePath != username && x.RelativePath != $"{username}\\archive" && x.UserId == userId).Select(x => new DirectorySelectBoxVM
           {
               Text = x.Name,
               Value = x.Id,
               ParentDirectoryId = x.ParentDirectoryId
           }).ToListAsync();
        }
    }
}
