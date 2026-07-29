using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Messages;

namespace Volo.Abp.MudBlazorUI.Components;

public partial class UiMessageAlert : ComponentBase, IDisposable
{
    private bool _visible;
    private readonly DialogOptions _dialogOptions = new()
    {
        CloseOnEscapeKey = false,
        BackdropClick = false,
        MaxWidth = MaxWidth.Small,
        FullWidth = true
    };

    protected virtual bool IsConfirmation
        => MessageType == UiMessageType.Confirmation;

    protected virtual bool CenterMessage
        => Options?.CenterMessage ?? true;

    protected virtual bool ShowMessageIcon
        => Options?.ShowMessageIcon ?? true;

    protected virtual bool IsMessageHtmlMarkup
        => Options?.IsMessageHtmlMarkup ?? false;

    protected virtual string MessageIcon => MessageType switch
    {
        UiMessageType.Info => Icons.Material.Filled.Info,
        UiMessageType.Success => Icons.Material.Filled.CheckCircle,
        UiMessageType.Warning => Icons.Material.Filled.Warning,
        UiMessageType.Error => Icons.Material.Filled.Error,
        UiMessageType.Confirmation => Icons.Material.Filled.Help,
        _ => Icons.Material.Filled.Info,
    };

    protected virtual Color MessageIconColor => MessageType switch
    {
        UiMessageType.Info => Color.Info,
        UiMessageType.Success => Color.Success,
        UiMessageType.Warning => Color.Warning,
        UiMessageType.Error => Color.Error,
        UiMessageType.Confirmation => Color.Default,
        _ => Color.Default,
    };

    protected virtual string OkButtonText
        => Options?.OkButtonText ?? "OK";

    protected virtual string ConfirmButtonText
        => Options?.ConfirmButtonText ?? "Confirm";

    protected virtual string CancelButtonText
        => Options?.CancelButtonText ?? "Cancel";

    [Parameter] public UiMessageType MessageType { get; set; }

    [Parameter] public string? Title { get; set; }

    [Parameter] public string Message { get; set; } = default!;

    [Parameter] public TaskCompletionSource<bool>? Callback { get; set; }

    [Parameter] public UiMessageOptions? Options { get; set; }

    [Parameter] public EventCallback Okayed { get; set; }

    [Parameter] public EventCallback Confirmed { get; set; }

    [Parameter] public EventCallback Canceled { get; set; }

    [Inject] protected MudBlazorUiMessageService? UiMessageService { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (UiMessageService != null)
        {
            UiMessageService.MessageReceived += OnMessageReceived;
        }
    }

    private async void OnMessageReceived(object? sender, UiMessageEventArgs e)
    {
        await InvokeAsync(async () =>
        {
            MessageType = e.MessageType;
            Message = e.Message;
            Title = e.Title;
            Options = e.Options;
            Callback = e.Callback;

            await ShowMessageAlert();
        });
    }

    protected virtual async Task ShowMessageAlert()
    {
        await InvokeAsync(() =>
        {
            _visible = true;
            StateHasChanged();
        });
    }

    protected virtual async Task HideMessageAlert()
    {
        await InvokeAsync(() =>
        {
            _visible = false;
            StateHasChanged();
        });
    }

    public void Dispose()
    {
        if (UiMessageService != null)
        {
            UiMessageService.MessageReceived -= OnMessageReceived;
        }
    }

    protected async Task OnOkClicked()
    {
        await HideMessageAlert();
        await Okayed.InvokeAsync(null);
    }

    protected async Task OnConfirmClicked()
    {
        await HideMessageAlert();

        if (IsConfirmation && Callback != null)
        {
            Callback.SetResult(true);
        }

        await Confirmed.InvokeAsync(null);
    }

    protected async Task OnCancelClicked()
    {
        await HideMessageAlert();

        if (IsConfirmation && Callback != null)
        {
            Callback.SetResult(false);
        }

        await Canceled.InvokeAsync(null);
    }
}
