using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.Web
{
    public class ComponentsComponentsBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context)
        {
            context.Add("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/abp.js");
        }

        public void AddStyles(BundleContext context)
        {

        }
    }
}
