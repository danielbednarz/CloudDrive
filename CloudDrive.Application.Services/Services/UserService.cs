using CloudDrive.Application.Abstraction;
using CloudDrive.Data.Abstraction;
using CloudDrive.Domain;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CloudDrive.Application
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IConfiguration config, IUserRepository userRepository, ITokenService tokenService)
        {
            _config = config;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public Task<bool> IsUserExists(string username)
        {
            return _userRepository.IsUserExists(username);
        }

        public async Task<UserDTO> Register(UserVM registerVM)
        {
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Username = registerVM.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerVM.Password)),
                PasswordSalt = hmac.Key
            };

            if (user == null)
            {
                return null;
            }

            _userRepository.Add(user);
            await _userRepository.SaveAsync();

            CreateDirectoryForNewUser(user.Username);

            return new UserDTO
            {
                Username = registerVM.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        private void CreateDirectoryForNewUser(string username)
        {
            var mainPath = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>().SaveFilePath;
            var pathForUser = Path.Combine(mainPath, username);

            try
            {
                var result = Directory.CreateDirectory(pathForUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<AppUser> GetUsers()
        {
            return _userRepository.GetAll().ToList();
        }

        public async Task<AppUser> GetUser(string username)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Username == username);

            return user;
        }

        public UserDTO Login(AppUser user, UserVM loginVM)
        {
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginVM.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return null;
                }
            }

            return new UserDTO
            {
                Username = loginVM.Username,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
