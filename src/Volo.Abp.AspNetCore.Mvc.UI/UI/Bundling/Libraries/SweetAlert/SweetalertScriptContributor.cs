using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Core;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.SweetAlert
{
    [DependsOn(typeof(CoreScriptContributor))]
    public class SweetalertScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/sweetalert/sweetalert.min.js");
        }
    }
}
