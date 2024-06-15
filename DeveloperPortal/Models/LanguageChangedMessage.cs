namespace DeveloperPortal.Models;

public class LanguageChangedMessage
{
    public LanguageChangedMessage(string newCulture)
    {
        NewCulture = newCulture;
    }

    public string NewCulture { get; }
}