using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.Bundling
{
    public interface IBundleManager
    {
        List<string> GetStyleBundleFiles(string bundleName);

        List<string> GetScriptBundleFiles(string bundleName);
    }
}