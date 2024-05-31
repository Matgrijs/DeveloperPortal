namespace DeveloperPortal.Models.Notes;

public class UpdateNoteDto(Guid id, string username, string auth0Id, string content)
{
    public Guid Id { get; set; } = id;
    public string Username { get; set; } = username;
    public string Auth0Id { get; set; } = auth0Id;
    public string Content { get; set; } = content;
}