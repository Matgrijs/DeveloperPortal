using Microsoft.AspNetCore.SignalR;

namespace DeveloperPortalApi.Hubs;

public class PokerHub : Hub
{
    public async Task AddInput(string username, string vote)
    {
        await Clients.All.SendAsync("ReceiveVote", username, vote);
    }
}