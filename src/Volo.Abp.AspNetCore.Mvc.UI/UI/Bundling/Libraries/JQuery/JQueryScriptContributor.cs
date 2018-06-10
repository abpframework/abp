namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.JQuery
{
    public class JQueryScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/jquery/jquery.js");
        }
    }
}
