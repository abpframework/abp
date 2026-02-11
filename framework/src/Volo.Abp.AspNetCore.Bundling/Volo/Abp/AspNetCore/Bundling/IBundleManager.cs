using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Bundling;

public interface IBundleManager
{
    Task<IReadOnlyList<BundleFile>> GetStyleBundleFilesAsync(string bundleName);

    Task<IReadOnlyList<BundleFile>> GetScriptBundleFilesAsync(string bundleName);
}
