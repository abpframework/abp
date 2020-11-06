using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme
{
    public class BasicThemeBundleContributer : IBundleContributer
    {
        public void AddScripts(BundleContext context)
        {

        }

        public void AddStyles(BundleContext context)
        {
            context.Add("_content/Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme/libs/abp/css/theme.css");
        }
    }
}
