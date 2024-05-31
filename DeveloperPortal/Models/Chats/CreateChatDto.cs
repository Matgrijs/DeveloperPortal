namespace DeveloperPortal.Models.Chats;

public class CreateChatDto
{
    public string Username { get; set; }
    public string Auth0Id { get; set; }
    public string Message { get; set; }
    public DateTimeOffset MessageTime { get; set; }
}