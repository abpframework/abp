using System.Threading.Tasks;
using Microsoft.JSInterop;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    //TODO: Implement with sweetalert in a new package
    public class UiMessageService : IUiMessageService, ITransientDependency
    {
        protected IJSRuntime JsRuntime { get; }

        public UiMessageService(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        public async Task InfoAsync(string message, string title = null)
        {
            await JsRuntime.InvokeVoidAsync("alert", message);
        }

        public async Task SuccessAsync(string message, string title = null)
        {
            await JsRuntime.InvokeVoidAsync("alert", message);
        }

        public async Task WarnAsync(string message, string title = null)
        {
            await JsRuntime.InvokeVoidAsync("alert", message);
        }
        
        public async Task ErrorAsync(string message, string title = null)
        {
            await JsRuntime.InvokeVoidAsync("alert", message);
        }
        
        public async Task<bool> ConfirmAsync(string message, string title = null)
        {
            return await JsRuntime.InvokeAsync<bool>("confirm", message);
        }
    }
}
