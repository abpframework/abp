using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Volo.Abp.MudBlazorUI.Components;

/// <summary>
/// A menu item that closes its menu before running <see cref="OnClick"/>, so a dialog opened by
/// the handler keeps the focus. Works inside an <see cref="AbpMudActionMenu"/> and inside a plain
/// <see cref="MudMenu"/>.
/// </summary>
public partial class AbpMudActionMenuItem : ComponentBase
{
    [CascadingParameter]
    protected AbpMudActionMenu? ParentMenu { get; set; }

    [CascadingParameter]
    protected MudMenu? ParentMudMenu { get; set; }

    /// <summary>
    /// Whether this item can close the menu itself. When it cannot, MudBlazor keeps closing it.
    /// </summary>
    protected virtual bool ControlsMenu => ParentMenu != null || ParentMudMenu != null;

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public Color IconColor { get; set; } = Color.Inherit;

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public string? Href { get; set; }

    [Parameter]
    public string? Target { get; set; }

    [Parameter]
    public bool ForceLoad { get; set; }

    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Never null, because MudBlazor reads it without a null check while rendering.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?> UserAttributes { get; set; } = new();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected virtual async Task OnClickHandlerAsync(MouseEventArgs args)
    {
        if (ParentMenu != null)
        {
            await ParentMenu.CloseAsync();
        }
        else if (ParentMudMenu != null)
        {
            await ParentMudMenu.CloseAllMenusAsync();
        }

        await OnClick.InvokeAsync(args);
    }
}
