using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme
{
    public class BundleContributer : BaseBundleContributer
    {
        public override string[] GetStyles()
        {
            return new string[]
            {
                "_content/Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme/libs/abp/css/theme.css",
            };
        }
    }
}
