using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class NullUiMessageService : IUiMessageService, ITransientDependency
    {
        public Task InfoAsync(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            return Task.CompletedTask;
        }

        public Task SuccessAsync(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            return Task.CompletedTask;
        }

        public Task WarnAsync(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            return Task.CompletedTask;
        }

        public Task ErrorAsync(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            return Task.CompletedTask;
        }

        public Task<bool> ConfirmAsync(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            return Task.FromResult(true);
        }
    }
}
