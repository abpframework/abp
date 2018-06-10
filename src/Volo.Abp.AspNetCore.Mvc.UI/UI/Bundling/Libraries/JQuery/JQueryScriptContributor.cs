using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Core;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQuery
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
