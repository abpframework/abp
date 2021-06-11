using System.Threading.Tasks;
using Microsoft.JSInterop;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web
{
    [Dependency(ReplaceServices = true)]
    public class CookieService : ICookieService, ITransientDependency
    {
        public IJSRuntime JsRuntime { get; }

        public CookieService(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        public async ValueTask SetAsync(string key, string value, CookieOptions options)
        {
            await JsRuntime.InvokeVoidAsync("abp.utils.setCookieValue", key, value, options?.ExpireDate?.ToString("r"), options?.Path, options?.Secure);
        }

        public async ValueTask<string> GetAsync(string key)
        {
            return await JsRuntime.InvokeAsync<string>("abp.utils.getCookieValue", key);
        }

        public async ValueTask DeleteAsync(string key, string path = null)
        {
            await JsRuntime.InvokeVoidAsync("abp.utils.deleteCookie", key);
        }
    }
}