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
        private readonly IDirectoryRepository _directoryRepository;

        public UserService(IConfiguration config, IUserRepository userRepository, ITokenService tokenService, IDirectoryRepository directoryRepository)
        {
            _config = config;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _directoryRepository = directoryRepository;
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

            await CreateDirectoryForNewUser(user.Username, user.Id);

            return new UserDTO
            {
                Username = registerVM.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task CreateDirectoryForNewUser(string username, int userId)
        {
            var mainPath = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>().SaveFilePath;
            var pathForUser = Path.Combine(mainPath, username);

            try
            {
                Directory.CreateDirectory(pathForUser);

                AddDirectoryVM model = new()
                { 
                    Name = username,
                    UserChosenPath = username,
                    UserId = userId
                };

                await _directoryRepository.AddDirectory(model);

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
