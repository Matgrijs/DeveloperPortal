namespace DeveloperPortal.Models.Chats;

public class CreateChatDto
{
    public required string? Username { get; set; }
    public required string? Auth0Id { get; set; }
    public required string Message { get; set; }
    public DateTimeOffset MessageTime { get; set; }
}