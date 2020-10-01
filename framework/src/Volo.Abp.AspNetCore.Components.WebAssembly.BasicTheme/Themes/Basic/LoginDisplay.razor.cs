using System;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.Http.Client;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Themes.Basic
{
    public partial class LoginDisplay : IDisposable
    {
        [Inject] 
        protected IMenuManager MenuManager { get; set; }
        
        [Inject]
        protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }
        
        [Inject]
        protected IOptions<AbpRemoteServiceOptions> RemoteServiceOptions { get; set; }
        
        protected ApplicationMenu Menu { get; set; }

        protected string ServerUrl { get; set; }
        protected string ServerAccountUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Menu = await MenuManager.GetAsync(StandardMenus.User);
            
            ServerUrl = RemoteServiceOptions.Value.RemoteServices.Default?.BaseUrl?.TrimEnd('/');
            ServerAccountUrl = ServerUrl + "/Account/Manage?returnUrl=" + Navigation.Uri;
            
            Navigation.LocationChanged += OnLocationChanged;
        }

        protected virtual void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            ServerAccountUrl = ServerUrl + "/Account/Manage?returnUrl=" + Navigation.Uri;
            StateHasChanged();
        }
        
        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }
    }
}