namespace DeveloperPortalApi.Entities;

public class Note
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Username { get; set; }
    public required string auth0Id { get; set; }
    public required string Content { get; set; }
}