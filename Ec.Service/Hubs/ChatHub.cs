using Microsoft.AspNetCore.SignalR;

namespace Ec.Service.Hubs;

public class ChatHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
}
