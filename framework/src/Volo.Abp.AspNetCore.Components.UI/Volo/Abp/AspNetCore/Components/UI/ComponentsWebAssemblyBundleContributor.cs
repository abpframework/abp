using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.UI
{
    public class ComponentsComponentsBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context)
        {
            context.Add("_content/Volo.Abp.AspNetCore.Components.UI/libs/abp/js/abp.js");
        }

        public void AddStyles(BundleContext context)
        {

        }
    }
}
