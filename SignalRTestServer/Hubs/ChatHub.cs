using Microsoft.AspNetCore.SignalR;
using SignalRTestServer.Models;
using System.Threading.Tasks;

namespace SignalRTestServer.Hubs
{
    public class ChatHub : Hub
    {
        public Task SendMessage(string user, Message message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
