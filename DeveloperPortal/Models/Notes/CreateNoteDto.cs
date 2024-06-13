namespace DeveloperPortal.Models.Notes;

public class CreateNoteDto
{
    public required string? Username { get; set; }
    public required string? Auth0Id { get; set; }
    public required string Content { get; set; }
}