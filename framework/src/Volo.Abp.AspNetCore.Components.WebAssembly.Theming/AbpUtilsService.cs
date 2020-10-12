using System.Threading.Tasks;
using Microsoft.JSInterop;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming
{
    public class AbpUtilsService : IAbpUtilsService, ITransientDependency
    {
        protected IJSRuntime JsRuntime { get; }

        public AbpUtilsService(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        public ValueTask AddClassToTagAsync(string tagName, string className)
        {
            return JsRuntime.InvokeVoidAsync("abp.utils.addClassToTag", tagName, className);
        }

        public ValueTask RemoveClassFromTagAsync(string tagName, string className)
        {
            return JsRuntime.InvokeVoidAsync("abp.utils.removeClassFromTag", tagName, className);
        }

        public ValueTask<bool> HasClassOnTagAsync(string tagName, string className)
        {
            return JsRuntime.InvokeAsync<bool>("abp.utils.hasClassOnTag", tagName, className);
        }

        public ValueTask ReplaceLinkHrefByIdAsync(string linkId, string hrefValue)
        {
            return JsRuntime.InvokeVoidAsync("abp.utils.replaceLinkHrefById", linkId, hrefValue);
        }
    }
}
