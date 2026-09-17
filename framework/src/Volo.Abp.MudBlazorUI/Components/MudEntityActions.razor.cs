using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Volo.Abp.MudBlazorUI.Components;

/// <summary>
/// Non-generic interface for MudEntityActions to allow cascading parameter matching
/// regardless of the generic type argument.
/// </summary>
public interface IMudEntityActions
{
    MudActionType Type { get; }
    void AddAction(object action);
}

public partial class MudEntityActions<TItem> : ComponentBase, IMudEntityActions
{
    protected readonly List<MudEntityAction<TItem>> Actions = new List<MudEntityAction<TItem>>();
    protected bool HasPrimaryAction => Actions.Any(t => t.Primary);
    protected MudEntityAction<TItem>? PrimaryAction => Actions.FirstOrDefault(t => t.Primary);

    [Parameter]
    public Color ToggleColor { get; set; } = Color.Primary;

    [Parameter]
    public string? ToggleText { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public MudDataGridEntityActionsColumn<TItem> EntityActionsColumn { get; set; } = default!;

    [Parameter]
    public MudActionType Type { get; set; } = MudActionType.Dropdown;

    [Parameter]
    public bool Disabled { get; set; } = false;

    [CascadingParameter]
    public MudDataGridEntityActionsColumn<TItem>? ParentEntityActionsColumn { get; set; }

    [Inject]
    public IStringLocalizer<AbpUiResource> UiLocalizer { get; set; } = default!;

    internal void AddAction(MudEntityAction<TItem> action)
    {
        Actions.Add(action);
    }

    void IMudEntityActions.AddAction(object action)
    {
        if (action is MudEntityAction<TItem> typedAction)
        {
            AddAction(typedAction);
        }
    }

    protected virtual bool DisabledOrNoActions()
    {
        return Disabled || (Actions.Any() && Actions.All(t => !t.Visible || !t.HasPermission));
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        ToggleText = UiLocalizer["Actions"];
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (ParentEntityActionsColumn != null)
            {
                ParentEntityActionsColumn.Displayable = Actions.Any(t => t.Visible && t.HasPermission);
            }

            await InvokeAsync(StateHasChanged);
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}

public enum MudActionType
{
    Dropdown,
    Button
}
