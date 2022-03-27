using CloudDrive.Domain;

namespace CloudDrive.Application
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
