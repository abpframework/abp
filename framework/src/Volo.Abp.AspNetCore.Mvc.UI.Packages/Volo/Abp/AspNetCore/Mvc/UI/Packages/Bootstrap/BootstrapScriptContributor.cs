using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class BootstrapScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/bootstrap/js/bootstrap.bundle.js");
            context.Files.AddIfNotContains("/libs/bootstrap/js/bootstrap.enable.tooltips.everywhere.js");
        }
    }
}
