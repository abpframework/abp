using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.RequestLocalization
{
    public interface IAbpRequestLocalizationOptionsProvider
    {
        Task<RequestLocalizationOptions> GetLocalizationOptionsAsync();
    }
}
