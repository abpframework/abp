using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Abp.Core;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Abp.JQuery
{
    [DependsOn(typeof(AbpCoreScriptContributor))]
    [DependsOn(typeof(JQueryScriptContributor))]
    public class AbpJQueryScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/abp/jquery/abp.jquery.js");
        }
    }
}
