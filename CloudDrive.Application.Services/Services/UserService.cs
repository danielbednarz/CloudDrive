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

            await CreateDirectoriesForNewUser(user);

            return new UserDTO
            {
                Username = registerVM.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task CreateDirectoriesForNewUser(AppUser user)
        {
            await CreateDirectory(user.Username, user.Id);                //add main directory
            await CreateDirectory("archive", user.Id, user.Username);     //add archive directory for deleted files
        }

        private async Task CreateDirectory(string directoryName, int userId, string path = null)
        {
            var mainPath = _config.GetSection("FileUploadConfig").Get<FileUploadConfig>().SaveFilePath;

            string pathForUser;
            AddDirectoryVM model;

            if (path == null)
            {
                pathForUser = Path.Combine(mainPath, directoryName);
                model = new()
                {
                    Name = directoryName,
                    GeneratedPath = directoryName,
                    UserId = userId,
                    ParentDirectoryId = null
                };
            }
            else
            {
                pathForUser = Path.Combine(mainPath, path, directoryName);
                model = new()
                {
                    Name = directoryName,
                    GeneratedPath = @$"{path}\{directoryName}",
                    UserId = userId,
                    ParentDirectoryId = _directoryRepository.FirstOrDefault(x => x.UserId == userId && x.ParentDirectoryId == null && x.Name == x.User.Username).Id,
                };
            }

            try
            {
                await _directoryRepository.AddDirectory(model);
                Directory.CreateDirectory(pathForUser);
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
