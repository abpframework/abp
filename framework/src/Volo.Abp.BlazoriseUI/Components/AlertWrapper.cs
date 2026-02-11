using Volo.Abp.AspNetCore.Components.Alerts;

namespace Volo.Abp.BlazoriseUI.Components;

internal class AlertWrapper
{
    public AlertMessage AlertMessage { get; set; } = default!;
    public bool IsVisible { get; set; }
}
