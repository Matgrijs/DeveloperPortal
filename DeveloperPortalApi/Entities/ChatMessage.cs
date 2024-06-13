namespace DeveloperPortalApi.Entities;

public class ChatMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public string auth0Id { get; set; }
    public string Message { get; set; }
    public DateTimeOffset MessageTime { get; set; }
}