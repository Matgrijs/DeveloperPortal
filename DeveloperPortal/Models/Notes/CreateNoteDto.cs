namespace DeveloperPortal.Models.Notes;

public class CreateNoteDto
{
    public string Username { get; set; }
    public string Auth0Id { get; set; }
    public string Content { get; set; }
}