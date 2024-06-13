using System.Threading.Tasks;
using Microsoft.JSInterop;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web;

[Dependency(ReplaceServices = true)]
public class LocalStorageService : ILocalStorageService, ITransientDependency
{
    public IJSRuntime JsRuntime { get; }
    
    public LocalStorageService(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }
    
    public async ValueTask SetItemAsync(string key, string value)
    {
        await JsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public async ValueTask<string> GetItemAsync(string key)
    {
        return await JsRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }

    public async ValueTask RemoveItemAsync(string key)
    {
        await JsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
}