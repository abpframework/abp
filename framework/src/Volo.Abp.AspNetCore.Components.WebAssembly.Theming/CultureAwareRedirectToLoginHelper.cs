using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.WebAssembly;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming;

/// <summary>
/// Provides the shared culture-aware login redirect logic for all WASM theme
/// <c>WebAssemblyRedirectToLogin</c> components. Each theme must keep
/// <c>@inherits RedirectToLogin</c> for ABP service-replacement assignability,
/// so a common component base class is not feasible; this static helper
/// centralises the logic instead.
/// </summary>
public static class CultureAwareRedirectToLoginHelper
{
    public static async Task RedirectAsync(
        NavigationManager navigation,
        string loginUrl,
        IRouteBasedCultureUrlHelper cultureUrlHelper,
        IOptions<AbpAspNetCoreComponentsWebOptions> webOptions)
    {
        var cultureLoginUrl = await cultureUrlHelper.PrependCulturePrefixAsync(loginUrl);
        if (webOptions.Value.IsBlazorWebApp)
        {
            navigation.NavigateTo(cultureLoginUrl, forceLoad: true);
        }
        else
        {
            navigation.NavigateToLogin(cultureLoginUrl);
        }
    }
}
