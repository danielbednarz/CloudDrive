using CloudDrive.Application;
using CloudDrive.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CloudDrive.Hubs
{
    public class FileHub : Hub
    {
        public IUserService _userService { get; set; }

        public FileHub(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        public override Task OnConnectedAsync()
        {
            AddToGroup(Context.GetHttpContext().Request.Query["username"]);
            return base.OnConnectedAsync();
        }

        private async Task AddToGroup(string groupName) => await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        public async Task RemoveFromGroup(string groupName) => await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
