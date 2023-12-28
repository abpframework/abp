using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public interface IBundleManager
{
    Task<IReadOnlyList<BundleFile>> GetStyleBundleFilesAsync(string bundleName);

    Task<IReadOnlyList<BundleFile>> GetScriptBundleFilesAsync(string bundleName);
}
