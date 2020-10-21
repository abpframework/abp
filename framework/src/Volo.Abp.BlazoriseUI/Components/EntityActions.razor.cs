using System.Collections.Generic;
using System.Linq;
using Blazorise;
using Microsoft.AspNetCore.Components;

namespace Volo.Abp.BlazoriseUI.Components
{
    public partial class EntityActions<TItem> : ComponentBase
    {
        protected List<EntityAction<TItem>> Actions = new List<EntityAction<TItem>>();
        protected bool HasPrimaryAction => Actions.Any(t => t.Primary);
        protected EntityAction<TItem> PrimaryAction => Actions.FirstOrDefault(t => t.Primary);
        protected internal ActionType Type => Actions.Count(t => t.IsVisible) > 1 ? ActionType.Dropdown : ActionType.Button;

        [Parameter]
        public Color ToggleColor { get; set; }

        [Parameter]
        public string ToggleText { get; set; }

        [Parameter]
        public TItem Entity { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal void AddAction(EntityAction<TItem> action)
        {
            Actions.Add(action);
            StateHasChanged();
        }
    }

    public enum ActionType
    {
        Dropdown,
        Button
    }
}
