using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQueryForm
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class JQueryFormScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/jquery-form/jquery.form.min.js");
        }
    }
}
