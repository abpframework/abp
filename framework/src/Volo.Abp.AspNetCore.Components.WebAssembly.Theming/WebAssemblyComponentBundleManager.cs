using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Bundling;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming;

public class WebAssemblyComponentBundleManager : IComponentBundleManager, ITransientDependency
{
    public virtual Task<IReadOnlyList<string>> GetStyleBundleFilesAsync(string bundleName)
    {
        return Task.FromResult<IReadOnlyList<string>>(new List<string>());
    }

    public virtual Task<IReadOnlyList<string>> GetScriptBundleFilesAsync(string bundleName)
    {
        return Task.FromResult<IReadOnlyList<string>>(new List<string>());
    }
}
