using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Popper;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap
{
    [DependsOn(typeof(JQueryScriptContributor))]
    [DependsOn(typeof(PopperJsScriptBundleContributor))]
    public class BootstrapScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/bootstrap/js/bootstrap.bundle.js");
        }
    }
}
