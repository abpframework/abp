using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class BundleContributer : IBundleContributer
    {
        public void AddScripts(List<string> scripts)
        {
            scripts.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.WebAssembly/libs/abp/js/abp.js");
        }

        public void AddStyles(List<string> styles)
        {

        }
    }
}
