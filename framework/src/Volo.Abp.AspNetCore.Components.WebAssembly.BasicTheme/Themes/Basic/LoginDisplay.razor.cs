using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Themes.Basic
{
    public partial class LoginDisplay : IDisposable
    {
        [Inject]
        protected IMenuManager MenuManager { get; set; }
        
        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        protected ApplicationMenu Menu { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Menu = await MenuManager.GetAsync(StandardMenus.User);

            Navigation.LocationChanged += OnLocationChanged;

            AuthenticationStateProvider.AuthenticationStateChanged += async (task) =>
            {
                Menu = await MenuManager.GetAsync(StandardMenus.User);
                await InvokeAsync(StateHasChanged);
            };
        }

        protected virtual void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }
    }
}
