using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DeveloperPortalApi.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string username, string message)
    {
        // Broadcast the message to all clients
        await Clients.All.SendAsync("ReceiveMessage", username, message);
    }
}