namespace DeveloperPortalApi.Entities;

public class ChatMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Username { get; set; }
    public required string auth0Id { get; set; }
    public required string Message { get; set; }
    public DateTimeOffset MessageTime { get; set; }
}