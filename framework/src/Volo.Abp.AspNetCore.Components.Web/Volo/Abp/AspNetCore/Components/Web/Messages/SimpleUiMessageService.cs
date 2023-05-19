using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.Messages;

public class SimpleUiMessageService : IUiMessageService, ITransientDependency
{
    protected IJSRuntime JsRuntime { get; }

    public SimpleUiMessageService(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }

    public async Task Info(string message, string title = null, Action<UiMessageOptions> options = null)
    {
        await JsRuntime.InvokeVoidAsync("alert", message);
    }

    public async Task Success(string message, string title = null, Action<UiMessageOptions> options = null)
    {
        await JsRuntime.InvokeVoidAsync("alert", message);
    }

    public async Task Warn(string message, string title = null, Action<UiMessageOptions> options = null)
    {
        await JsRuntime.InvokeVoidAsync("alert", message);
    }

    public async Task Error(string message, string title = null, Action<UiMessageOptions> options = null)
    {
        await JsRuntime.InvokeVoidAsync("alert", message);
    }

    public async Task<bool> Confirm(string message, string title = null, Action<UiMessageOptions> options = null)
    {
        return await JsRuntime.InvokeAsync<bool>("confirm", message);
    }
}
