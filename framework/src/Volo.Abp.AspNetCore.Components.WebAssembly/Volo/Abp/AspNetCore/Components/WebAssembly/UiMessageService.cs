using System.Threading.Tasks;
using Microsoft.JSInterop;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class UiMessageService : IUiMessageService, ITransientDependency
    {
        protected IJSRuntime JsRuntime { get; }

        public UiMessageService(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        public async Task<bool> ConfirmAsync(string message, string title = null)
        {
            //TODO: Implement with sweetalert in a new package
            return await JsRuntime.InvokeAsync<bool>("confirm", message);
        }
    }
}
