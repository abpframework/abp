using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlazoriseUI
{
    [Dependency(ReplaceServices = true)]
    public class UiMessageNotifierService : IScopedDependency
    {
        public event EventHandler<UiMessageEventArgs> MessageReceived;

        public Task NotifyMessageReceivedAsync(UiMessageType messageType, string message, string title, UiMessageOptions options, TaskCompletionSource<bool> callback = null)
        {
            MessageReceived?.Invoke(this, new UiMessageEventArgs(messageType, message, title, options, callback));

            return Task.CompletedTask;
        }
    }
}
