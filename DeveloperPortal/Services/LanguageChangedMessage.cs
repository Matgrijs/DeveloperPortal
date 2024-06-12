namespace DeveloperPortal.Services;

public class LanguageChangedMessage
{
    public string NewCulture { get; }

    public LanguageChangedMessage(string newCulture)
    {
        NewCulture = newCulture;
    }
}