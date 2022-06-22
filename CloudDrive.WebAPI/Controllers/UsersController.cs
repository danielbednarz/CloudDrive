using CloudDrive.Application;
using CloudDrive.Domain;
using CloudDrive.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CloudDrive.WebAPI.Controllers
{
    public class UsersController : AppController
    {
        private readonly IUserService _userService;
        public readonly IHubContext<FileHub> _hubContext;

        public UsersController(IUserService userService, IHubContext<FileHub> hubContext)
        {
            _userService = userService;
            _hubContext = hubContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(UserVM registerVM)
        {
            if (await _userService.IsUserExists(registerVM.Username.ToLower()))
            {
                return BadRequest("Nazwa użytkownika jest zajęta");
            }

            var registeredUser = await _userService.Register(registerVM);

            return registeredUser == null ? BadRequest("Błąd przy próbie utworzenia użytkownika") : registeredUser;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserVM loginVM)
        {
            var user = await _userService.GetUser(loginVM.Username);

            if (user == null)
            {
                return Unauthorized("Nieprawidłowa nazwa użytkownika");
            }

            var loggedUser = _userService.Login(user, loginVM);

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
            return _userService.GetUsers();
        }
        
        [Authorize]
        [HttpGet("getUser")]
        public async Task<ActionResult<AppUser>> GetUser(string username)
        {
            var user = await _userService.GetUser(username);

            return user == null ? NotFound("Brak użytkownika o podanej nazwie") : user;

        }
        
        [HttpGet("checkConnection")]
        public async Task<ActionResult> CheckConnection()
        {
            return Ok();
        }
    }
}
