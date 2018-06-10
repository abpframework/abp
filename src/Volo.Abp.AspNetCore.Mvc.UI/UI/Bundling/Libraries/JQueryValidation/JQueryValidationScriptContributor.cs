using Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQueryValidation
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class JQueryValidationScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/jquery-validation/jquery.validate.js");
        }
    }
}
