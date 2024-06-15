namespace DeveloperPortal.Models.SentryErrors;

public class SentryError
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Culprit { get; set; }
}