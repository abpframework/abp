using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic;

public partial class SecondLevelNavMenuItem : IDisposable
{
    [Inject] private NavigationManager NavigationManager { get; set; }

    [Parameter]
    public ApplicationMenuItem MenuItem { get; set; }

    public bool IsSubMenuOpen { get; set; }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void ToggleSubMenu()
    {
        IsSubMenuOpen = !IsSubMenuOpen;
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    private void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        IsSubMenuOpen = false;
        InvokeAsync(StateHasChanged);
    }
}
