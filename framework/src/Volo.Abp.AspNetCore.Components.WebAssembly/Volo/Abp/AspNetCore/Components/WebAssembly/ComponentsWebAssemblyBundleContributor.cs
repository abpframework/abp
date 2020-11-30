using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class ComponentsWebAssemblyBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context)
        {
            context.Add("_content/Volo.Abp.AspNetCore.Components.WebAssembly/libs/abp/js/abp.js");
        }

        public void AddStyles(BundleContext context)
        {

        }
    }
}
