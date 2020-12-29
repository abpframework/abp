using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.RequestLocalization
{
    public interface IAbpRequestLocalizationOptionsProvider
    {
        void InitLocalizationOptions(Action<RequestLocalizationOptions> optionsAction = null);

        Task<RequestLocalizationOptions> GetLocalizationOptionsAsync();
    }
}
