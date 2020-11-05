using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming
{
    public class BundleContributer : BaseBundleContributer
    {
        public override string[] GetStyles()
        {
            return new string[]
            {
                "_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/libs/bootstrap/css/bootstrap.min.css",
                "_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/libs/fontawesome/css/all.css",
            };
        }
    }
}
