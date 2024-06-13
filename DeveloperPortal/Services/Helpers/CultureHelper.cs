using System.Globalization;

namespace DeveloperPortal.Services;

public static class CultureHelper
{
    private static string _currentCultureCode = "en-US";

    public static string CurrentCultureCode
    {
        get => _currentCultureCode;
        set
        {
            _currentCultureCode = value;
            CultureInfo.CurrentCulture = new CultureInfo(_currentCultureCode);
            CultureInfo.CurrentUICulture = new CultureInfo(_currentCultureCode);
        }
    }

    public static bool IsDutchEnabled => CurrentCultureCode == "nl-NL";
}