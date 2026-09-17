using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Alerts;

namespace Volo.Abp.MudBlazorUI.Components;

public partial class PageAlert : ComponentBase, IDisposable
{
    private List<AlertWrapper> Alerts = new List<AlertWrapper>();

    [Inject]
    protected IAlertManager AlertManager { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

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
    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        AlertManager.Alerts.Clear();
        Alerts.Clear();
    }

    private void Alerts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems!)
            {
                Alerts.Add(new AlertWrapper
                {
                    AlertMessage = (AlertMessage)item,
                    IsVisible = true
                });
            }
        }
        InvokeAsync(StateHasChanged);
    }

    protected virtual Severity GetAlertSeverity(AlertType alertType)
    {
        return alertType switch
        {
            AlertType.Info => Severity.Info,
            AlertType.Success => Severity.Success,
            AlertType.Warning => Severity.Warning,
            AlertType.Danger => Severity.Error,
            _ => Severity.Normal,
        };
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
        AlertManager.Alerts.CollectionChanged -= Alerts_CollectionChanged;
    }
}
