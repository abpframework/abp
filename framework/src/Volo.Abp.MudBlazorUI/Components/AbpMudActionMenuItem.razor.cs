using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Volo.Abp.MudBlazorUI.Components;

/// <summary>
/// An item of an <see cref="AbpMudActionMenu"/>. It closes the menu before running
/// <see cref="OnClick"/>, so a dialog opened by the handler keeps the focus.
/// Outside an <see cref="AbpMudActionMenu"/> it behaves like a plain <see cref="MudMenuItem"/>.
/// </summary>
public partial class AbpMudActionMenuItem : ComponentBase
{
    [CascadingParameter]
    protected AbpMudActionMenu? ParentMenu { get; set; }

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

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UserAttributes { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected virtual async Task OnClickHandlerAsync(MouseEventArgs args)
    {
        if (ParentMenu != null)
        {
            await ParentMenu.CloseAsync();
        }

        await OnClick.InvokeAsync(args);
    }
}
