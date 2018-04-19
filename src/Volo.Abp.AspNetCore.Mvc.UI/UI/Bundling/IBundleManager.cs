using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IBundleManager
    {
        List<string> GetStyleBundleFiles(string bundleName);

        List<string> GetScriptBundleFiles(string bundleName);
    }
}