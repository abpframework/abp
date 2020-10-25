using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.WebAssembly;

namespace Volo.Abp.BlazoriseUI.Components
{
    public partial class EntityAction<TItem> : ComponentBase
    {
        internal bool IsVisible;

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public bool Primary { get; set; }

        [Parameter]
        public EventCallback Clicked { get; set; }

        [Parameter]
        public string Policy { get; set; }

        [Parameter]
        public Color Color { get; set; }
        
        [Parameter] 
        public string ConfirmationMessage { get; set; }

        [CascadingParameter]
        public EntityActions<TItem> ParentActions { get; set; }

        [Inject]
        protected IAuthorizationService AuthorizationService { get; set; }

        [Inject]
        protected IUiMessageService UiMessageService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await SetDefaultValuesAsync();
            IsVisible = await AuthorizationService.IsGrantedAsync(Policy);
            ParentActions.AddAction(this);
        }

        protected internal virtual async Task ActionClickedAsync()
        {
            if (!ConfirmationMessage.IsNullOrEmpty())
            {
                if (await UiMessageService.ConfirmAsync(ConfirmationMessage))
                {
                    await Clicked.InvokeAsync();
                }
            }
            else
            {
                await Clicked.InvokeAsync();
            }
        }

        protected virtual ValueTask SetDefaultValuesAsync()
        {
            Color = Color.Primary;
            return ValueTask.CompletedTask;
        }
    }
}
