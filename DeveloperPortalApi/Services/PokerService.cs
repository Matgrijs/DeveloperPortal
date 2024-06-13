using DeveloperPortalApi.Data;
using DeveloperPortalApi.Entities;
using DeveloperPortalApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DeveloperPortalApi.Services;

public class PokerService(IHubContext<PokerHub> hubContext, ApplicationDbContext dbContext)
{
    public async Task AddVote(PokerVote pokerVote)
    {
        dbContext.PokerVotes.Add(pokerVote);
        await dbContext.SaveChangesAsync();

        // Notify all connected clients
        await hubContext.Clients.All.SendAsync("ReceiveVote", pokerVote);
    }

    public async Task UpdateVote(PokerVote pokerVote)
    {
        dbContext.PokerVotes.Update(pokerVote);
        await dbContext.SaveChangesAsync();

        // Notify all connected clients
        await hubContext.Clients.All.SendAsync("ReceiveVote", pokerVote);
    }
}