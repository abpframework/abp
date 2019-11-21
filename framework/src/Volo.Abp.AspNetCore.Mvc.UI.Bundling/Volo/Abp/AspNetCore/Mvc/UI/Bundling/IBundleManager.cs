using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IBundleManager
    {
        IReadOnlyList<string> GetStyleBundleFiles(string bundleName);

        IReadOnlyList<string> GetScriptBundleFiles(string bundleName);
    }
}