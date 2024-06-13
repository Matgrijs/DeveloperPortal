namespace DeveloperPortal.Models.Chats;

public class ChatMessage
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Auth0Id { get; set; }
    public required string Message { get; set; }
    public DateTimeOffset MessageTime { get; set; }
}