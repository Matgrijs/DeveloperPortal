using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace DeveloperPortal.Services.Interfaces
{
    public interface ISentryService
    {
        Task<JArray> GetSentryErrors();
    }
}