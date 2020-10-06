using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlazoriseUI
{
    [Dependency(ReplaceServices = true)]
    public class BlazoriseUiMessageService : IUiMessageService, IScopedDependency
    {
        private readonly IUiMessageNotifierService uiMessageNotifierService;

        public ILogger<BlazoriseUiMessageService> Logger { get; set; }

        public BlazoriseUiMessageService(IUiMessageNotifierService uiMessageNotifierService)
        {
            this.uiMessageNotifierService = uiMessageNotifierService;

            Logger = NullLogger<BlazoriseUiMessageService>.Instance;
        }

        public Task InfoAsync(string message, string title = null)
        {
            return uiMessageNotifierService.NotifyMessageReceivedAsync(UiMessageType.Info, message, title);
        }

        public Task SuccessAsync(string message, string title = null)
        {
            return uiMessageNotifierService.NotifyMessageReceivedAsync(UiMessageType.Success, message, title);
        }

        public Task WarnAsync(string message, string title = null)
        {
            return uiMessageNotifierService.NotifyMessageReceivedAsync(UiMessageType.Warning, message, title);
        }

        public Task ErrorAsync(string message, string title = null)
        {
            return uiMessageNotifierService.NotifyMessageReceivedAsync(UiMessageType.Error, message, title);
        }

        public async Task<bool> ConfirmAsync(string message, string title = null)
        {
            var callback = new TaskCompletionSource<bool>();

            await uiMessageNotifierService.NotifyConfirmationReceivedAsync(message, title, callback);

            return await callback.Task;
        }
    }
}
