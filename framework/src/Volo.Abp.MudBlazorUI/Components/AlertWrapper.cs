using Volo.Abp.AspNetCore.Components.Alerts;

namespace Volo.Abp.MudBlazorUI.Components;

internal class AlertWrapper
{
    public AlertMessage AlertMessage { get; set; } = default!;
    public bool IsVisible { get; set; }
}
