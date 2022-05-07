using Microsoft.EntityFrameworkCore;
using CloudDrive.Domain;

namespace CloudDrive.EntityFramework
{
    public class MainDatabaseContext : DbContext
    {
        public MainDatabaseContext(DbContextOptions options) : base(options)
        {

        }

        //Add-Migration -Context MainDatabaseContext -o Migrations/MainDatabaseMigrations <Nazwa migracji>
        //Update-Database -Context MainDatabaseContext
        //Remove-Migration -Context MainDatabaseContext
        // dotnet ef migrations add Init -o Data\Migrations
        // dotnet ef database update

        public DbSet<UserFile> Files { get; set; }
        public DbSet<FileOperationsLogs> FileOperationsLogs { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserFile>().HasKey(x => x.Id);
            modelBuilder.Entity<FileOperationsLogs>().HasKey(x => x.Id);
        }
    }
}
