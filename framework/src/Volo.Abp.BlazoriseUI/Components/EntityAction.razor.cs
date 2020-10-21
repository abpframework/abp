using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Volo.Abp.BlazoriseUI.Components
{
    public partial class EntityAction<TItem> : ComponentBase
    {
        [Parameter]
        public bool IsVisible { get; set; }
        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public bool Primary { get; set; }

        [Parameter]
        public EventCallback<TItem> Clicked { get; set; }

        [CascadingParameter]
        public EntityActions<TItem> ParentActions { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ParentActions.AddAction(this);
        }

        protected virtual async Task ActionClickedAsync()
        {
            await Clicked.InvokeAsync(ParentActions.Entity);
        }
    }
}
