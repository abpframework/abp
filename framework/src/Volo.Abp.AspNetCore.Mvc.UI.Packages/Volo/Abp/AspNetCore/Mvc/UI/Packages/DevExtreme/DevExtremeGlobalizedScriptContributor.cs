using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Globalize;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JsZip;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.DevExtreme
{
    [DependsOn(typeof(JsZipScriptContributor))]
    [DependsOn(typeof(GlobalizeScriptContributor))]
    public class DevExtremeGlobalizedScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/devextreme/dist/js/dx.all.js");
        }
    }
}
