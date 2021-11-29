using System;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Messages
{
    public class UiMessageEventArgs : EventArgs
    {
        public UiMessageEventArgs(UiMessageType messageType, string message, string title, UiMessageOptions options)
        {
            MessageType = messageType;
            Message = message;
            Title = title;
            Options = options;
        }

        public UiMessageEventArgs(UiMessageType messageType, string message, string title, UiMessageOptions options, TaskCompletionSource<bool> callback)
        {
            MessageType = messageType;
            Message = message;
            Title = title;
            Options = options;
            Callback = callback;
        }

        public UiMessageType MessageType { get; set; }

        public string Message { get; }

        public string Title { get; }

        public UiMessageOptions Options { get; }

        public TaskCompletionSource<bool> Callback { get; }
    }
}
