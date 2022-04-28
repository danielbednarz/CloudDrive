﻿using CloudDrive.Application;
using CloudDrive.Data.Interfaces;
using CloudDrive.Domain;
using CloudDrive.EntityFramework;

namespace CloudDrive.Data.Repositories
{
    public class FileRepository : GenericRepository<UserFile>, IFileRepository
    {
        public FileRepository(MainDatabaseContext context) : base(context)
        {
        }

        public async Task AddFile(AddUserFileVM file)
        {
            var fileName = file.File.FileName;
            long fileVersion = 0;

            if (_context.Files.Any(x => x.Name == fileName))
            {
                fileVersion = _context.Files.Where(y => y.Name == fileName)
                    .Select(x => x.FileVersion).ToList().Max();

                fileVersion += 1;
            }

            _context.Files.Add(new UserFile
            {
                Name = fileName,
                Size = file.File.Length,
                FileVersion = fileVersion,
                CreatedDate = DateTime.Now,
            });

            await _context.SaveChangesAsync();
        }
    }
}