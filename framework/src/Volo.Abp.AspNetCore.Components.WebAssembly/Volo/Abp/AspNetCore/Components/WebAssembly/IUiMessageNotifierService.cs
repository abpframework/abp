using System;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public interface IUiMessageNotifierService
    {
        event EventHandler<UiMessageEventArgs> MessageReceived;

        Task NotifyMessageReceived(string message, string title = null);
    }

    public class UiMessageEventArgs : EventArgs
    {
        public UiMessageEventArgs(string message, string title)
        {
            Message = message;
            Title = title;
        }

        public string Message { get; }

        public string Title { get; }
    }
}
