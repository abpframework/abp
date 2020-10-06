using System;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public interface IUiMessageNotifierService
    {
        event EventHandler<UiMessageEventArgs> MessageReceived;

        Task NotifyMessageReceivedAsync(UiMessageType messageType, string message, string title = null);

        Task NotifyConfirmationReceivedAsync(string message, string title, TaskCompletionSource<bool> callback);
    }

    public class UiMessageEventArgs : EventArgs
    {
        public UiMessageEventArgs(UiMessageType messageType, string message, string title)
        {
            MessageType = messageType;
            Message = message;
            Title = title;
        }

        public UiMessageEventArgs(UiMessageType messageType, string message, string title, TaskCompletionSource<bool> callback)
        {
            MessageType = messageType;
            Message = message;
            Title = title;
            Callback = callback;
        }

        public UiMessageType MessageType { get; set; }

        public string Message { get; }

        public string Title { get; }

        public TaskCompletionSource<bool> Callback { get; }
    }

    public enum UiMessageType
    {
        Info,
        Success,
        Warning,
        Error,
        Confirmation,
    }
}
