namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Core
{
    public class CoreScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/abp/core/abp.js");
        }
    }
}
