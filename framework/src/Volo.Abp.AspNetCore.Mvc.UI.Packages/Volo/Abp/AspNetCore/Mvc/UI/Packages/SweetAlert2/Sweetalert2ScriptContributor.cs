using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Core;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.SweetAlert2
{
    [DependsOn(typeof(CoreScriptContributor))]
    public class Sweetalert2ScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/sweetalert2/sweetalert2.all.min.js");
        }
    }
}
