using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Themes.Basic
{
    public partial class LoginDisplay : IDisposable
    {
        [Inject]
        protected IMenuManager MenuManager { get; set; }

        [CanBeNull]
        protected AuthenticationStateProvider AuthenticationStateProvider;

        [CanBeNull]
        protected SignOutSessionStateManager SignOutManager;

        protected ApplicationMenu Menu { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            Menu = await MenuManager.GetAsync(StandardMenus.User);

            Navigation.LocationChanged += OnLocationChanged;

            LazyGetService(ref AuthenticationStateProvider);
            LazyGetService(ref SignOutManager);

            if (AuthenticationStateProvider != null)
            {
                AuthenticationStateProvider.AuthenticationStateChanged +=
                    AuthenticationStateProviderOnAuthenticationStateChanged;
            }
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
            if (AuthenticationStateProvider != null)
            {
                AuthenticationStateProvider.AuthenticationStateChanged -=
                    AuthenticationStateProviderOnAuthenticationStateChanged;
            }
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

        private async Task BeginSignOut()
        {
            if (SignOutManager != null)
            {
                await SignOutManager.SetSignOutState();
                await NavigateToAsync("authentication/logout");
            }
        }
    }
}
