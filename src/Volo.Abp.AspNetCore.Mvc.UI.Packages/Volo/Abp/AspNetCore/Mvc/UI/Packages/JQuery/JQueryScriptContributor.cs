using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Core;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery
{
    [DependsOn(typeof(CoreScriptContributor))]
    public class JQueryScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/jquery/jquery.js");
            context.Files.Add("/libs/abp/jquery/abp.jquery.js");
        }
    }
}
