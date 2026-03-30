using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Themes.Basic;

public partial class LoginDisplay : IDisposable
{
    [Inject]
    protected IMenuManager MenuManager { get; set; }

    [Inject]
    protected ApplicationConfigurationChangedService ApplicationConfigurationChangedService { get; set; }

    [Inject]
    protected ICachedApplicationConfigurationClient ConfigurationClient { get; set; }

    protected ApplicationMenu Menu { get; set; }

    private ApplicationConfigurationDto _config;

    protected string LoginUrl => PrependCulturePrefix(AuthenticationOptions.Value.LoginUrl);

    protected string LogoutUrl => PrependCulturePrefix(AuthenticationOptions.Value.LogoutUrl);

    protected string PrependCulturePrefix(string url)
    {
        if (_config?.Localization.UseRouteBasedCulture != true)
        {
            return url;
        }

        var currentCulture = CultureInfo.CurrentCulture.Name;
        var isKnownCulture = _config.Localization.Languages
            .Any(l => string.Equals(l.CultureName, currentCulture, StringComparison.OrdinalIgnoreCase));

        return isKnownCulture ? $"{currentCulture}/{url}" : url;
    }

    protected async override Task OnInitializedAsync()
    {
        _config = await ConfigurationClient.GetAsync();

        Menu = await MenuManager.GetAsync(StandardMenus.User);

        Navigation.LocationChanged += OnLocationChanged;

        ApplicationConfigurationChangedService.Changed += ApplicationConfigurationChanged;
    }

    protected virtual void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    private async void ApplicationConfigurationChanged()
    {
        Menu = await MenuManager.GetAsync(StandardMenus.User);
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
        ApplicationConfigurationChangedService.Changed -= ApplicationConfigurationChanged;
    }

    private async Task NavigateToAsync(string uri, string target = null)
    {
        uri = uri?.TrimStart('~', '/') ?? uri;

        if (target == "_blank")
        {
            await JsRuntime.InvokeVoidAsync("open", Navigation.ToAbsoluteUri(uri).ToString(), target);
        }
        else
        {
            Navigation.NavigateTo(uri);
        }
    }

    private void BeginSignOut()
    {
        if (AbpAspNetCoreComponentsWebOptions.Value.IsBlazorWebApp)
        {
            Navigation.NavigateTo(LogoutUrl, forceLoad: true);
        }
        else
        {
            Navigation.NavigateToLogout(LogoutUrl);
        }
    }
}
