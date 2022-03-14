using CloudDrive.Data.Interfaces;
using CloudDrive.Data.Repositories;
using CloudDrive.Domain;
using CloudDrive.EntityFramework;

namespace CloudDrive.Data.Repositories
{
    public class FileRepository : GenericRepository<UserFile>, IFileRepository
    {
        //public MainDatabaseContext MainDatabaseContext { get; set; }
        public FileRepository(MainDatabaseContext context) : base(context)
        {
        }


        public void AddFile()
        {
            _context.Files.Add(new UserFile
            {
                CreatedDate = DateTime.Now,
                Name = "Plik testowy",
                Size = 256
            });

            _context.SaveChanges();
        }
    }
}
