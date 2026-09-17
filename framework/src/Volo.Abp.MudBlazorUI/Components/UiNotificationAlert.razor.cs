using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Notifications;
using Volo.Abp.Localization;

namespace Volo.Abp.MudBlazorUI.Components;

public partial class UiNotificationAlert : ComponentBase, IDisposable
{
    [Parameter] public UiNotificationType NotificationType { get; set; }

    [Parameter] public string Message { get; set; } = default!;

    [Parameter] public string? Title { get; set; }

    [Parameter] public UiNotificationOptions? Options { get; set; }

    [Parameter] public EventCallback Okayed { get; set; }

    [Parameter] public EventCallback Closed { get; set; }

    [Inject] protected MudBlazorUiNotificationService? UiNotificationService { get; set; }

    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    [Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; } = default!;

    protected virtual Severity GetSeverity(UiNotificationType notificationType)
    {
        return notificationType switch
        {
            UiNotificationType.Info => Severity.Info,
            UiNotificationType.Success => Severity.Success,
            UiNotificationType.Warning => Severity.Warning,
            UiNotificationType.Error => Severity.Error,
            _ => Severity.Normal,
        };
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (UiNotificationService != null)
        {
            UiNotificationService.NotificationReceived += OnNotificationReceived;
        }
    }

    protected virtual async void OnNotificationReceived(object? sender, UiNotificationEventArgs e)
    {
        NotificationType = e.NotificationType;
        Message = e.Message;
        Title = e.Title;
        Options = e.Options;

        var message = !string.IsNullOrEmpty(Title)
            ? $"<strong>{Title}</strong><br/>{Message}"
            : Message;

        await InvokeAsync(() =>
        {
            Snackbar.Add(message, GetSeverity(e.NotificationType), config =>
            {
                config.RequireInteraction = false;
                config.ShowCloseIcon = true;
                config.CloseAfterNavigation = true;
            });
        });
    }

    public virtual void Dispose()
    {
        if (UiNotificationService != null)
        {
            UiNotificationService.NotificationReceived -= OnNotificationReceived;
        }
    }
}
