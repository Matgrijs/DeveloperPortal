namespace DeveloperPortal.Models.Users;

public class User
{
    public required string? Name { get; set; }
    public required string? Auth0Id { get; set; }
    public string? Vote { get; set; }
}