using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Volo.Abp.AspNetCore.Components.Alerts;

namespace Volo.Abp.BlazoriseUI.Components;

public partial class PageAlert : ComponentBase, IDisposable
{
    private List<AlertWrapper> Alerts = new List<AlertWrapper>();

    [Inject]
    protected IAlertManager AlertManager { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        NavigationManager.LocationChanged += NavigationManager_LocationChanged;
        AlertManager.Alerts.CollectionChanged += Alerts_CollectionChanged;

        Alerts.AddRange(AlertManager.Alerts.Select(t => new AlertWrapper
        {
            AlertMessage = t,
            IsVisible = true
        }));
    }

    //Since Blazor WASM doesn't support scoped dependency, we need to clear alerts on each location changed event.
    private void NavigationManager_LocationChanged(object sender, LocationChangedEventArgs e)
    {
        AlertManager.Alerts.Clear();
        Alerts.Clear();
    }

    private void Alerts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems)
            {
                Alerts.Add(new AlertWrapper
                {
                    AlertMessage = (AspNetCore.Components.Alerts.AlertMessage)item,
                    IsVisible = true
                });
            }
        }
        InvokeAsync(StateHasChanged);
    }

    protected virtual Color GetAlertColor(AlertType alertType)
    {
        var color = alertType switch
        {
            AlertType.Info => Color.Info,
            AlertType.Success => Color.Success,
            AlertType.Warning => Color.Warning,
            AlertType.Danger => Color.Danger,
            AlertType.Dark => Color.Dark,
            AlertType.Default => Color.Default,
            AlertType.Light => Color.Light,
            AlertType.Primary => Color.Primary,
            AlertType.Secondary => Color.Secondary,
            _ => Color.Default,
        };

        return color;
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
        AlertManager.Alerts.CollectionChanged -= Alerts_CollectionChanged;
    }
}
