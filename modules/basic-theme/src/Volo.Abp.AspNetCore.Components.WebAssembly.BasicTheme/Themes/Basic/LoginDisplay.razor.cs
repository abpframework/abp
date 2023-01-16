using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Themes.Basic;

public partial class LoginDisplay : IDisposable
{
    [Inject]
    protected IMenuManager MenuManager { get; set; }

    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    protected ApplicationMenu Menu { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Menu = await MenuManager.GetAsync(StandardMenus.User);

        Navigation.LocationChanged += OnLocationChanged;

        AuthenticationStateProvider.AuthenticationStateChanged +=
            AuthenticationStateProviderOnAuthenticationStateChanged;
    }

    protected virtual void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    private async void AuthenticationStateProviderOnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        Menu = await MenuManager.GetAsync(StandardMenus.User);
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
        AuthenticationStateProvider.AuthenticationStateChanged -=
            AuthenticationStateProviderOnAuthenticationStateChanged;
    }

    private async Task NavigateToAsync(string uri, string target = null)
    {
        if (target == "_blank")
        {
            await JsRuntime.InvokeVoidAsync("open", uri, target);
        }
        else
        {
            Navigation.NavigateTo(uri);
        }
    }

    private void BeginSignOut()
    {
        Navigation.NavigateToLogout("authentication/logout");
    }
}
