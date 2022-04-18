using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public interface IUserService
    {
        Task<bool> IsUserExists(string username);
        Task<UserDTO> Register(UserVM registerVM);
        UserDTO Login(AppUser user, UserVM loginVM);
        List<AppUser> GetUsers();
        Task<AppUser> GetUser(string username);
    }
}
