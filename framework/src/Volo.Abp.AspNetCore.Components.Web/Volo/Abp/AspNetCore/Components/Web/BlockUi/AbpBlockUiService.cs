using System.Threading.Tasks;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.BlockUi;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.BlockUi;

[Dependency(ReplaceServices = true)]
public class AbpBlockUiService : IBlockUiService, IScopedDependency
{
    public IJSRuntime JsRuntime { get; }

    public AbpBlockUiService(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }

    public async Task Block(string? selectors, bool busy = false)
    {
        await JsRuntime.InvokeVoidAsync("abp.ui.block", selectors, busy);
    }

    public async Task UnBlock()
    {
        await JsRuntime.InvokeVoidAsync("abp.ui.unblock");
    }
}
