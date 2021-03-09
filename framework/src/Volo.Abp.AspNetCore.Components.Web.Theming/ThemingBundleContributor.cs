using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.Web.Theming
{
    public class ThemingBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context)
        {

        }

        public void AddStyles(BundleContext context)
        {
            context.BundleDefinitions.Insert(0, new BundleDefinition
            {
                Source = "_content/Volo.Abp.AspNetCore.Components.Web.Theming/libs/bootstrap/css/bootstrap.min.css"
            });
            context.BundleDefinitions.Insert(1, new BundleDefinition
            {
                Source = "_content/Volo.Abp.AspNetCore.Components.Web.Theming/libs/fontawesome/css/all.css"
            });
            context.Add("_content/Volo.Abp.AspNetCore.Components.Web.Theming/libs/flag-icon/css/flag-icon.css");
        }
    }
}
