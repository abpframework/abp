using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme
{
    public class BasicThemeBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context, BundleParameterDictionary parameters)
        {

        }

        public void AddStyles(BundleContext context, BundleParameterDictionary parameters)
        {
            context.Add("_content/Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme/libs/abp/css/theme.css");
        }
    }
}
