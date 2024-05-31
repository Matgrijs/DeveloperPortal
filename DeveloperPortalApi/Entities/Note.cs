namespace DeveloperPortalApi.Entities;

public class Note
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public string auth0Id { get; set; }
    public string Content { get; set; }
}
