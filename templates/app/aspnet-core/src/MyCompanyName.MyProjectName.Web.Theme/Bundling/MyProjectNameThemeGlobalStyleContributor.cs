using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MyCompanyName.MyProjectName.Web.Theme.Bundling
{
    public class MyProjectNameThemeGlobalStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/themes/MyProjectName/layout.css");
        }
    }
}
