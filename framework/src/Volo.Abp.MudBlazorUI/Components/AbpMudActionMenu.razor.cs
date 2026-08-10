using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Volo.Abp.MudBlazorUI.Components;

/// <summary>
/// A <see cref="MudMenu"/> for entity/row actions that open a dialog.
/// Use it together with <see cref="AbpMudActionMenuItem"/>.
/// </summary>
public partial class AbpMudActionMenu : ComponentBase
{
    protected MudMenu? _menu;

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public Color IconColor { get; set; } = Color.Inherit;

    [Parameter]
    public string? StartIcon { get; set; }

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter]
    public Color Color { get; set; } = Color.Default;

    [Parameter]
    public Variant Variant { get; set; } = Variant.Text;

    [Parameter]
    public Size Size { get; set; } = Size.Medium;

    [Parameter]
    public bool Dense { get; set; }

    [Parameter]
    public Origin? AnchorOrigin { get; set; }

    [Parameter]
    public Origin TransformOrigin { get; set; } = Origin.TopLeft;

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    /// <summary>
    /// Replaces the default activator button. The menu is opened through the given
    /// <see cref="MenuContext"/>, same as <see cref="MudMenu.ActivatorContent"/>.
    /// </summary>
    [Parameter]
    public RenderFragment<MenuContext>? ActivatorContent { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Closes the menu and returns the focus to its activator.
    /// <see cref="AbpMudActionMenuItem"/> calls this before running its handler: MudBlazor restores
    /// the focus while closing the menu, and doing that after the handler opened a dialog would take
    /// the focus back out of that dialog.
    /// </summary>
    public virtual async Task CloseAsync()
    {
        if (_menu != null)
        {
            await _menu.CloseAllMenusAsync();
        }
    }
}
