using System.Runtime.InteropServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DeveloperPortal.WinUI;

/// <summary>
///     Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    public static MauiWinUIWindow? CurrentWindow { get; set; }
    /// <summary>
    ///     Initializes the singleton application object.  This is the first line of authored code
    ///     executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        // Authentication
        if (
            WinUIEx.WebAuthenticator.CheckOAuthRedirectionActivation())
            return;

        this.InitializeComponent();
    }
    // Authentication
    public static void ActivateApplication()
    {
        if (CurrentWindow != null)
        {
            try
            {
                SetForegroundWindow(CurrentWindow.WindowHandle);
            }
            catch
            {
            }
        }
    }
    
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    
    // Authentication
    [DllImport("DeveloperPortal.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);
}