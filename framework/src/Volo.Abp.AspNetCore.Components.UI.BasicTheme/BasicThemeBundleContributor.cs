using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.UI.BasicTheme
{
    public class BasicThemeBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context)
        {

        }

        public void AddStyles(BundleContext context)
        {
            context.Add("_content/Volo.Abp.AspNetCore.Components.UI.BasicTheme/libs/abp/css/theme.css");
        }
    }
}
