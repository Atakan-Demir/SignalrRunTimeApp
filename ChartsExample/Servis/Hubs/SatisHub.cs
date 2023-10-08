using Microsoft.AspNetCore.SignalR;

namespace Servis.Hubs
{
    public class SatisHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("reciveMessage", "Merhaba");
        }
    }
}
