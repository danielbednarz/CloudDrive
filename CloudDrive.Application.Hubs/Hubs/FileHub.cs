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
        public override async Task OnConnectedAsync()
        {
            await AddToGroup(Context.GetHttpContext().Request.Query["username"]);
            await base.OnConnectedAsync();
        }

        private async Task AddToGroup(string groupName) => await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        public async Task RemoveFromGroup(string groupName) => await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
