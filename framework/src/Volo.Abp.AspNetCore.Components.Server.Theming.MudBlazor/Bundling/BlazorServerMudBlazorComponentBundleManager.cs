using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Bundling;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor.Bundling;

public class BlazorServerMudBlazorComponentBundleManager : IComponentBundleManager, ITransientDependency
{
    protected IBundleManager BundleManager { get; }

    public BlazorServerMudBlazorComponentBundleManager(IBundleManager bundleManager)
    {
        BundleManager = bundleManager;
    }

    public virtual async Task<IReadOnlyList<string>> GetStyleBundleFilesAsync(string bundleName)
    {
        return (await BundleManager.GetStyleBundleFilesAsync(bundleName)).Select(f => f.FileName).ToList();
    }

    public virtual async Task<IReadOnlyList<string>> GetScriptBundleFilesAsync(string bundleName)
    {
        return (await BundleManager.GetScriptBundleFilesAsync(bundleName)).Select(f => f.FileName).ToList();
    }
}