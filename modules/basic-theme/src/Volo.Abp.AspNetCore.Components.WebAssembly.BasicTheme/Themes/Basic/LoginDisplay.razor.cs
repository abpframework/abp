using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Themes.Basic;

public partial class LoginDisplay : IDisposable
{
    [Inject]
    protected IMenuManager MenuManager { get; set; }

    [Inject]
    protected ApplicationConfigurationChangedService ApplicationConfigurationChangedService { get; set; }

    [Inject]
    protected IRouteBasedCultureUrlHelper CultureUrlHelper { get; set; }

    protected ApplicationMenu Menu { get; set; }

    protected string LoginUrl { get; set; } = string.Empty;

    protected string LogoutUrl { get; set; } = string.Empty;

    protected async override Task OnInitializedAsync()
    {
        LoginUrl = await CultureUrlHelper.PrependCulturePrefixAsync(AuthenticationOptions.Value.LoginUrl);
        LogoutUrl = await CultureUrlHelper.PrependCulturePrefixAsync(AuthenticationOptions.Value.LogoutUrl);

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
        LoginUrl = await CultureUrlHelper.PrependCulturePrefixAsync(AuthenticationOptions.Value.LoginUrl);
        LogoutUrl = await CultureUrlHelper.PrependCulturePrefixAsync(AuthenticationOptions.Value.LogoutUrl);
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

    private async Task BeginSignOut()
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
