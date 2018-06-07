using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public interface IBundleManager
    {
        List<string> GetStyleBundleFiles(string bundleName);

        List<string> GetScriptBundleFiles(string bundleName);

        void CreateDynamicStyleBundle(string bundleName, Action<BundleConfiguration> configureAction);

        void CreateDynamicScriptBundle(string bundleName, Action<BundleConfiguration> configureAction);
    }
}