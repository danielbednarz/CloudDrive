using CloudDrive.Application;
using CloudDrive.Data.Abstraction;
using CloudDrive.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CloudDrive.WebAPI.Controllers
{
    public class UsersController : AppController
    {
        public IUserRepository UserRepository { get; set; }
        public ITokenService TokenService { get; set; }

        public UsersController(IUserRepository userRepository, ITokenService tokenService)
        {
            UserRepository = userRepository;
            TokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(UserVM registerVM)
        {
            registerVM.Username = registerVM.Username.ToLower();

            if (await UserRepository.IsUserExists(registerVM.Username))
            {
                return BadRequest("Nazwa użytkownika jest zajęta");
            }

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Username = registerVM.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerVM.Password)),
                PasswordSalt = hmac.Key
            };

            UserRepository.Add(user);

            await UserRepository.SaveAsync();

            return new UserDTO
            {
                Username = registerVM.Username,
                Token = TokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserVM loginVM)
        {
            var user = await UserRepository.FirstOrDefaultAsync(x => x.Username == loginVM.Username);

            if (user == null)
            {
                return Unauthorized("Nieprawidłowa nazwa użytkownika");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginVM.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Nieprawidłowe hasło");
                }
            }

            return new UserDTO
            {
                Username = loginVM.Username,
                Token = TokenService.CreateToken(user)
            };
        }

        [Authorize]
        [HttpGet("getUsers")]
        public ActionResult<List<AppUser>> GetUsers()
        {
            return UserRepository.GetAll().ToList();
        }

        [Authorize]
        [HttpGet("getUser")]
        public ActionResult<AppUser> GetUser(string username)
        {
            var user = UserRepository.FirstOrDefault(x => x.Username == username);
            return user == null ? NotFound("Brak użytkownika o podanej nazwie") : user;
        }
    }
}
