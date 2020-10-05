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

        public Task NotifyMessageReceived(string message, string title = null)
        {
            MessageReceived?.Invoke(this, new UiMessageEventArgs(message, title));

            return Task.CompletedTask;
        }
    }
}
