using DeveloperPortalApi.Data;
using DeveloperPortalApi.Entities;
using DeveloperPortalApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DeveloperPortalApi.Services;

public class ChatService(IHubContext<ChatHub> hubContext, ApplicationDbContext dbContext)
{
    public async Task AddMessage(ChatMessage chatMessage)
    {
        chatMessage.MessageTime = chatMessage.MessageTime.UtcDateTime;
        dbContext.ChatMessages.Add(chatMessage);
        await dbContext.SaveChangesAsync();
        
        // Notify all connected clients
        await hubContext.Clients.All.SendAsync("ReceiveMessage", chatMessage);
    }
}
