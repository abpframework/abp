using System;
using System.Threading.Tasks;
using Blazorise;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.WebAssembly;

namespace Volo.Abp.BlazoriseUI.Components
{
    public partial class EntityAction<TItem> : ComponentBase
    {
        internal bool IsVisible = true;

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public bool Primary { get; set; }

        [Parameter]
        public EventCallback Clicked { get; set; }

        [Parameter]
        public string RequiredPolicy { get; set; }

        [Parameter]
        public Color Color { get; set; }

        [Parameter]
        public Func<string> ConfirmationMessage { get; set; }

        [CascadingParameter]
        public EntityActions<TItem> ParentActions { get; set; }

        [Inject]
        protected IAuthorizationService AuthorizationService { get; set; }

        [Inject]
        protected IUiMessageService UiMessageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await SetDefaultValuesAsync();
            if (!RequiredPolicy.IsNullOrEmpty())
            {
                IsVisible = await AuthorizationService.IsGrantedAsync(RequiredPolicy);
            }
            ParentActions.AddAction(this);
        }

        protected internal virtual async Task ActionClickedAsync()
        {
            if (ConfirmationMessage != null)
            {
                if (await UiMessageService.Confirm(ConfirmationMessage()))
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
