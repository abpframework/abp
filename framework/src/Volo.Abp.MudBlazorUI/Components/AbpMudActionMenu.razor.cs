using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Volo.Abp.MudBlazorUI.Components;

/// <summary>
/// A <see cref="MudMenu"/> for actions that open a dialog. Use it with <see cref="AbpMudActionMenuItem"/>.
/// </summary>
public partial class AbpMudActionMenu : ComponentBase
{
    protected MudMenu? _menu;

    [Inject]
    protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; } = default!;

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public Color IconColor { get; set; } = Color.Inherit;

    [Parameter]
    public string? StartIcon { get; set; }

    [Parameter]
    public string? EndIcon { get; set; }

    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// The accessible name of an activator that has no <see cref="Label"/>. Defaults to "Actions".
    /// </summary>
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
    public bool FullWidth { get; set; }

    [Parameter]
    public int? MaxHeight { get; set; }

    [Parameter]
    public Origin? AnchorOrigin { get; set; }

    [Parameter]
    public Origin TransformOrigin { get; set; } = Origin.TopLeft;

    [Parameter]
    public MouseEvent ActivationEvent { get; set; } = MouseEvent.LeftClick;

    [Parameter]
    public bool PositionAtCursor { get; set; }

    [Parameter]
    public bool PopoverFixed { get; set; }

    [Parameter]
    public DropdownWidth RelativeWidth { get; set; } = DropdownWidth.Ignore;

    [Parameter]
    public bool LockScroll { get; set; }

    [Parameter]
    public bool Ripple { get; set; } = true;

    [Parameter]
    public bool DropShadow { get; set; } = true;

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public string? ListClass { get; set; }

    [Parameter]
    public string? PopoverClass { get; set; }

    /// <summary>
    /// Never null, because MudBlazor reads it without a null check while rendering.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?> UserAttributes { get; set; } = new();

    [Parameter]
    public RenderFragment<MenuContext>? ActivatorContent { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected virtual string? GetAriaLabel()
    {
        if (!AriaLabel.IsNullOrEmpty())
        {
            return AriaLabel;
        }

        // A label is already the accessible name, so overriding it would hide the visible text.
        return Label.IsNullOrEmpty() ? UiLocalizer["Actions"].Value : null;
    }

    /// <summary>
    /// Closes the whole menu hierarchy. MudBlazor restores the focus to the activator while closing,
    /// so <see cref="AbpMudActionMenuItem"/> calls this before its handler opens a dialog.
    /// </summary>
    public virtual async Task CloseAsync()
    {
        if (_menu != null)
        {
            await InvokeAsync(() => _menu.CloseAllMenusAsync());
        }
    }
}
