using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    [Dependency(ReplaceServices = true)]
    public class CookieService : ICookieService, ITransientDependency
    {
        public IJSRuntime JsRuntime { get; }

        public CookieService(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        public async ValueTask SetAsync(string key, string value, DateTimeOffset? expireDate = null, string path = null)
        {
            await JsRuntime.InvokeVoidAsync("abp.utils.setCookieValue", key, value, expireDate?.ToString("r"), path);
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