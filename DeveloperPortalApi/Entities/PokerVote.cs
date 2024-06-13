namespace DeveloperPortalApi.Entities;

public class PokerVote
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Username { get; set; }
    public required string Vote { get; set; }
}