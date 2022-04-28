using CloudDrive.Domain;

namespace CloudDrive.Data.Abstraction
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        Task<bool> IsUserExists(string username);
    }
}
