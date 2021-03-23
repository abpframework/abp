﻿using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;

namespace Volo.Abp.BlazoriseUI.Components
{
    public partial class EntityAction<TItem> : ComponentBase
    {
        [Parameter] 
        public bool IsVisible { get; set; } = true;

        internal bool HasPermission { get; set; } = true;

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public bool Primary { get; set; }

        [Parameter]
        public EventCallback Clicked { get; set; }

        [Parameter]
        [Obsolete("Use IsVisible to hide actions based on permissions. Check the permission yourself. It is more performant. This option might be removed in future versions.")]
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
                HasPermission = await AuthorizationService.IsGrantedAsync(RequiredPolicy);
            }
            ParentActions.AddAction(this);
        }

        protected internal virtual async Task ActionClickedAsync()
        {
            if (ConfirmationMessage != null)
            {
                if (await UiMessageService.Confirm(ConfirmationMessage()))
                {
                    await InvokeAsync(async () => await Clicked.InvokeAsync());
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
