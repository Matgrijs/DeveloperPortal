namespace DeveloperPortal.Services
{
    public class ApiService
    {
        public string BaseUrl { get; }

        public ApiService()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                BaseUrl = $"http://10.0.2.2:5186";
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                BaseUrl = $"https://localhost:7059";
            }
            else
            {
                BaseUrl = $"https://localhost:7059";
            }
        }
    }
}