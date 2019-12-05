using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IBundleManager
    {
        Task<IReadOnlyList<string>> GetStyleBundleFilesAsync(string bundleName);

        Task<IReadOnlyList<string>> GetScriptBundleFilesAsync(string bundleName);
    }
}