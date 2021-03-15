using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MyCompanyName.MyProjectName.Web.Theme.Bundling
{
    public class MyProjectNameThemeGlobalScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/themes/MyProjectName/layout.js");
        }
    }
}
