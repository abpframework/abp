using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Http.Client.Authentication;
using Microsoft.JSInterop;
using System.Reflection.Metadata;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.WebApp;

public class PersistentComponentStateAbpAccessTokenProvider : IAbpAccessTokenProvider
{
    private readonly IJSRuntime jSRuntime;

    public PersistentComponentStateAbpAccessTokenProvider(IJSRuntime jSRuntime)
    {
        this.jSRuntime = jSRuntime;
    }

    public virtual async Task<string?> GetTokenAsync()
    {
        var token = await jSRuntime.InvokeAsync<string>("getCookie", "access_token"); // TODO: localstorage.getItem('access_token')

        Console.WriteLine("PersistentComponentStateAbpAccessTokenProvider.GetTokenAsync: " + token);

        return token;
    }
}
