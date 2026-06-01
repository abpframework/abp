using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Messages;

namespace Volo.Abp.MudBlazorUI.Components;

public partial class MudEntityAction<TItem> : ComponentBase
{
    [Parameter]
    public bool Visible { get; set; } = true;

    [Parameter]
    public bool Disabled { get; set; } = false;

    internal bool HasPermission { get; set; } = true;

    [Parameter]
    public string Text { get; set; } = default!;

    [Parameter]
    public bool Primary { get; set; }

    [Parameter]
    public EventCallback Clicked { get; set; }

    [Parameter]
    [Obsolete("Use Visible to hide actions based on permissions. Check the permission yourself. It is more performant. This option might be removed in future versions.")]
    public string? RequiredPolicy { get; set; }

    [Parameter]
    public Color Color { get; set; } = Color.Primary;

    [Parameter]
    public Func<string>? ConfirmationMessage { get; set; }

    [Parameter]
    public string? Icon { get; set; }

    [CascadingParameter(Name = "ParentActions")]
    public IMudEntityActions? ParentActions { get; set; } = default!;

    [Inject]
    protected IAuthorizationService AuthorizationService { get; set; } = default!;

    [Inject]
    protected IUiMessageService UiMessageService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetDefaultValuesAsync();

#pragma warning disable CS0618
        if (!RequiredPolicy.IsNullOrEmpty())
        {
            HasPermission = await AuthorizationService.IsGrantedAsync(RequiredPolicy!);
        }
#pragma warning restore CS0618

        ParentActions?.AddAction(this);
    }

    protected internal virtual async Task ActionClickedAsync()
    {
        if (ConfirmationMessage != null)
        {
            if (await UiMessageService.Confirm(ConfirmationMessage()))
            {
                await InvokeAsync(() => Clicked.InvokeAsync());
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
