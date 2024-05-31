namespace DeveloperPortal.Models.Notes;

public class Note(Guid id, string username, string auth0Id, string content)
{
    public Guid Id { get; } = id;
    public string Username { get; init; } = username;
    public string Auth0Id { get; } = auth0Id;
    public string Content { get; init; } = content;
}