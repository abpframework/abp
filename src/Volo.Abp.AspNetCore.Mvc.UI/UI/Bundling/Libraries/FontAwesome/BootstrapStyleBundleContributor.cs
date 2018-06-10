namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Libraries.FontAwesome
{
    public class FontAwesomeStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/font-awesome/css/font-awesome.css");
        }
    }
}