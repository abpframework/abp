using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.BlazoriseUI.Components;

public partial class EntityActions<TItem> : ComponentBase
{
    protected readonly List<EntityAction<TItem>> Actions = new List<EntityAction<TItem>>();
    protected bool HasPrimaryAction => Actions.Any(t => t.Primary);
    protected EntityAction<TItem> PrimaryAction => Actions.FirstOrDefault(t => t.Primary);

    [Parameter]
    public Color ToggleColor { get; set; } = Color.Primary;

<<<<<<< HEAD
        [Parameter]
        public Size Size { get; set; } = Size.Medium;

        [Parameter]
        public string ToggleText { get; set; }
=======
    [Parameter]
    public string ToggleText { get; set; }
>>>>>>> 56157b3e60540adc0c0ccc7fe00b8005ae02044f

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public DataGridEntityActionsColumn<TItem> EntityActionsColumn { get; set; }

    [Parameter]
    public ActionType Type { get; set; } = ActionType.Dropdown;

    [CascadingParameter]
    public DataGridEntityActionsColumn<TItem> ParentEntityActionsColumn { get; set; }

    [Inject]
    public IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }

    internal void AddAction(EntityAction<TItem> action)
    {
        Actions.Add(action);
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
