using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.BlazoriseUI.Components
{
    public partial class EntityActions : ComponentBase
    {
        protected readonly List<EntityAction> Actions = new List<EntityAction>();
        protected bool HasPrimaryAction => Actions.Any(t => t.Primary);
        protected EntityAction PrimaryAction => Actions.FirstOrDefault(t => t.Primary);
        protected internal ActionType Type => Actions.Count(t => t.IsVisible) > 1 ? ActionType.Dropdown : ActionType.Button;

        [Parameter]
        public Color ToggleColor { get; set; }

        [Parameter]
        public string ToggleText { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        public IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }

        internal void AddAction(EntityAction action)
        {
            Actions.Add(action);
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await SetDefaultValuesAsync();
        }

        protected virtual ValueTask SetDefaultValuesAsync()
        {
            ToggleColor = Color.Primary;
            ToggleText = UiLocalizer["Actions"];
            return ValueTask.CompletedTask;
        }
    }

    public enum ActionType
    {
        Dropdown,
        Button
    }
}
