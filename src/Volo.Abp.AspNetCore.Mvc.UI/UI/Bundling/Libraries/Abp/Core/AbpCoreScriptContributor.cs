namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.Abp.Core
{
    public class AbpCoreScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/abp/core/abp.js");
        }
    }
}
