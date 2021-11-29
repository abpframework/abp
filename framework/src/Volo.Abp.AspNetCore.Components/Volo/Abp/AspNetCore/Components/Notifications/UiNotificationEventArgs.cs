using System;

namespace Volo.Abp.AspNetCore.Components.Notifications
{
    public class UiNotificationEventArgs : EventArgs
    {
        public UiNotificationEventArgs(UiNotificationType notificationType, string message, string title, UiNotificationOptions options)
        {
            NotificationType = notificationType;
            Message = message;
            Title = title;
            Options = options;
        }

        public UiNotificationType NotificationType { get; set; }

        public string Message { get; }

        public string Title { get; }

        public UiNotificationOptions Options { get; }
    }
}
