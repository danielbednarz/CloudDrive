using CloudDrive.Application;
using Microsoft.AspNetCore.SignalR;

namespace CloudDrive.Hubs
{
    public class FileHub : Hub<IFileHub>
    {
        //public async Task FileAdded()
        //{
        //    await Clients.All.LiveChatMessageReceived("test2");
        //}
    }
}
