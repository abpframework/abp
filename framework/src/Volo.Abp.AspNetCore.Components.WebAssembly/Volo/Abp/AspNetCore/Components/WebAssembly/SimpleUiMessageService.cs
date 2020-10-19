using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class SimpleUiMessageService : IUiMessageService, ITransientDependency
    {
        protected IJSRuntime JsRuntime { get; }

        public SimpleUiMessageService(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        public async Task InfoAsync(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            await JsRuntime.InvokeVoidAsync("alert", message);
        }

        public async Task SuccessAsync(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            await JsRuntime.InvokeVoidAsync("alert", message);
        }

        public async Task WarnAsync(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            await JsRuntime.InvokeVoidAsync("alert", message);
        }

        public async Task ErrorAsync(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            await JsRuntime.InvokeVoidAsync("alert", message);
        }

        public async Task<bool> ConfirmAsync(string message, string title = null, Action<UiMessageOptions> options = null)
        {
            return await JsRuntime.InvokeAsync<bool>("confirm", message);
        }
    }
}
