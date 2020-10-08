using System;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlazoriseUI
{
    [Dependency(ReplaceServices = true)]
    public class BlazoriseUiMessageService : IUiMessageService, IScopedDependency
    {
        private readonly UiMessageNotifierService uiMessageNotifierService;
        private readonly IStringLocalizer<AbpUiResource> _localizer;

        public ILogger<BlazoriseUiMessageService> Logger { get; set; }

        public BlazoriseUiMessageService(
            UiMessageNotifierService uiMessageNotifierService,
            IStringLocalizer<AbpUiResource> localizer)
        {
            this.uiMessageNotifierService = uiMessageNotifierService;
            _localizer = localizer;

            Logger = NullLogger<BlazoriseUiMessageService>.Instance;
        }

        public Task InfoAsync(string message, string title = null, Action<UiMessageOptions> optionsActions = null)
        {
            var options = CreateDefaultOptions();
            optionsActions?.Invoke(options);
            return uiMessageNotifierService.NotifyMessageReceivedAsync(UiMessageType.Info, message, title, options);
        }

        protected virtual UiMessageOptions CreateDefaultOptions()
        {
            return new UiMessageOptions {
                ConfirmButtonText = _localizer["Yes"]
            };
        }

        public Task SuccessAsync(string message, string title = null, UiMessageOptions options = null)
        {
            return uiMessageNotifierService.NotifyMessageReceivedAsync(UiMessageType.Success, message, title, options);
        }

        public Task WarnAsync(string message, string title = null, UiMessageOptions options = null)
        {
            return uiMessageNotifierService.NotifyMessageReceivedAsync(UiMessageType.Warning, message, title, options);
        }

        public Task ErrorAsync(string message, string title = null, UiMessageOptions options = null)
        {
            return uiMessageNotifierService.NotifyMessageReceivedAsync(UiMessageType.Error, message, title, options);
        }

        public async Task<bool> ConfirmAsync(string message, string title = null, UiMessageOptions options = null)
        {
            var callback = new TaskCompletionSource<bool>();

            await uiMessageNotifierService.NotifyMessageReceivedAsync(UiMessageType.Confirmation, message, title, options, callback);

            return await callback.Task;
        }
    }
}
