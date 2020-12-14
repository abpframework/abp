using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming
{
    public class ThemingBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context, BundleParameterDictionary parameters)
        {

        }

        public void AddStyles(BundleContext context, BundleParameterDictionary parameters)
        {
            context.BundleDefinitions.Insert(0, new BundleDefinition
            {
                Source = "_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/libs/bootstrap/css/bootstrap.min.css"
            });
            context.BundleDefinitions.Insert(1, new BundleDefinition
            {
                Source = "_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/libs/fontawesome/css/all.css"
            });
            context.Add("_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/libs/flag-icon/css/flag-icon.css");
        }
    }
}
