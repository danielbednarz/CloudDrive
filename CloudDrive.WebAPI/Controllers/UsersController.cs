using CloudDrive.Application;
using CloudDrive.Application.Abstraction;
using CloudDrive.Data.Abstraction;
using CloudDrive.Domain;
using CloudDrive.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CloudDrive.WebAPI.Controllers
{
    public class UsersController : AppController
    {
        public IUserService UserService { get; set; }
        public readonly IHubContext<FileHub> _hubContext;

        public UsersController(IUserService userService, IHubContext<FileHub> hubContext)
        {
            UserService = userService;
            _hubContext = hubContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(UserVM registerVM)
        {
            if (await UserService.IsUserExists(registerVM.Username.ToLower()))
            {
                return BadRequest("Nazwa użytkownika jest zajęta");
            }

            var registeredUser = await UserService.Register(registerVM);

            return registeredUser == null ? BadRequest("Błąd przy próbie utworzenia użytkownika") : registeredUser;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserVM loginVM)
        {
            var user = await UserService.GetUser(loginVM.Username);

            if (user == null)
            {
                return Unauthorized("Nieprawidłowa nazwa użytkownika");
            }

            var loggedUser = UserService.Login(user, loginVM);

            if (loggedUser != null)
            {
                return loggedUser;
            }
            else
            {
                return Unauthorized("Nieprawidłowe hasło");
            }
        }

        [Authorize]
        [HttpGet("getUsers")]
        public ActionResult<List<AppUser>> GetUsers()
        {
            return UserService.GetUsers();
        }
        
        [Authorize]
        [HttpGet("getUser")]
        public async Task<ActionResult<AppUser>> GetUser(string username)
        {
            var user = await UserService.GetUser(username);

            return user == null ? NotFound("Brak użytkownika o podanej nazwie") : user;

        }
        
        [HttpGet("checkConnection")]
        public async Task<ActionResult> CheckConnection()
        {
            return Ok();
        }
    }
}
