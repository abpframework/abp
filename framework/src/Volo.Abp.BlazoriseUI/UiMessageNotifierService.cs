using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlazoriseUI
{
    [Dependency(ReplaceServices = true)]
    public class UiMessageNotifierService : IUiMessageNotifierService, IScopedDependency
    {
        public event EventHandler<UiMessageEventArgs> MessageReceived;

        public Task NotifyMessageReceivedAsync(UiMessageType messageType, string message, string title = null)
        {
            MessageReceived?.Invoke(this, new UiMessageEventArgs(messageType, message, title));

            return Task.CompletedTask;
        }

        public Task NotifyConfirmationReceivedAsync(string message, string title, TaskCompletionSource<bool> callback)
        {
            MessageReceived?.Invoke(this, new UiMessageEventArgs(UiMessageType.Confirmation, message, title, callback));

            return Task.CompletedTask;
        }
    }
}
