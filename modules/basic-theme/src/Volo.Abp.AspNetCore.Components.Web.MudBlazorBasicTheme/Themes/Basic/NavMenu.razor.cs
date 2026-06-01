using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme.Themes.Basic;

public partial class NavMenu : IDisposable
{
    [Inject]
    protected IMenuManager MenuManager { get; set; }

    [Inject]
    protected ApplicationConfigurationChangedService ApplicationConfigurationChangedService { get; set; }

    protected ApplicationMenu Menu { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Menu = await MenuManager.GetMainMenuAsync();
        ApplicationConfigurationChangedService.Changed += ApplicationConfigurationChanged;
    }

    private void ApplicationConfigurationChanged()
    {
        _ = InvokeAsync(async () =>
        {
            Menu = await MenuManager.GetMainMenuAsync();
            StateHasChanged();
        });
    }

    public void Dispose()
    {
        ApplicationConfigurationChangedService.Changed -= ApplicationConfigurationChanged;
    }
}
