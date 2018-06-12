using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    //TODO: Bundle system needs refactoring/redesign

    public interface IBundleManager
    {
        List<string> GetStyleBundleFiles(string bundleName);

        List<string> GetScriptBundleFiles(string bundleName);

        void CreateStyleBundle(string bundleName, Action<BundleConfiguration> configureAction);

        void CreateScriptBundle(string bundleName, Action<BundleConfiguration> configureAction);
    }
}