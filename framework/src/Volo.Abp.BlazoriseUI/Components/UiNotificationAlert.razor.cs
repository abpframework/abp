using System;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Components.Notifications;

namespace Volo.Abp.BlazoriseUI.Components;

public partial class UiNotificationAlert : ComponentBase, IDisposable
{
    protected SnackbarStack SnackbarStack { get; set; }

    [Parameter] public UiNotificationType NotificationType { get; set; }

    [Parameter] public string Message { get; set; }

    [Parameter] public string Title { get; set; }

    [Parameter] public UiNotificationOptions Options { get; set; }

    [Parameter] public EventCallback Okayed { get; set; }

    [Parameter] public EventCallback Closed { get; set; }

    [Inject] protected BlazoriseUiNotificationService UiNotificationService { get; set; }

    [Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }

    protected virtual SnackbarColor GetSnackbarColor(UiNotificationType notificationType)
    {
        return notificationType switch
        {
            UiNotificationType.Info => SnackbarColor.Info,
            UiNotificationType.Success => SnackbarColor.Success,
            UiNotificationType.Warning => SnackbarColor.Warning,
            UiNotificationType.Error => SnackbarColor.Danger,
            _ => SnackbarColor.None,
        };
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        UiNotificationService.NotificationReceived += OnNotificationReceived;
    }

    protected virtual async void OnNotificationReceived(object sender, UiNotificationEventArgs e)
    {
        NotificationType = e.NotificationType;
        Message = e.Message;
        Title = e.Title;
        Options = e.Options;

        var okButtonText = Options?.OkButtonText?.Localize(StringLocalizerFactory);

        await SnackbarStack.PushAsync(Message, Title, GetSnackbarColor(e.NotificationType), (options) =>
      {
          options.CloseButtonIcon = IconName.Times;
          options.ActionButtonText = okButtonText;
      });
    }

    public virtual void Dispose()
    {
        if (UiNotificationService != null)
        {
            UiNotificationService.NotificationReceived -= OnNotificationReceived;
        }
    }

    protected virtual Task OnSnackbarClosed(SnackbarClosedEventArgs eventArgs)
    {
        return eventArgs.CloseReason == SnackbarCloseReason.UserClosed
            ? Okayed.InvokeAsync()
            : Closed.InvokeAsync();
    }
}
