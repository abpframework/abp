using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class BundleContributer : BaseBundleContributer
    {
        public override string[] GetScripts()
        {
            return new string[]
            {
                "_content/Volo.Abp.AspNetCore.Components.WebAssembly/libs/abp/js/abp.js",
            };
        }
    }
}
