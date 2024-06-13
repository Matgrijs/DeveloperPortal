namespace DeveloperPortal.Models.SentryErrors;

public class SentryError
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Culprit { get; set; }
}