using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class BundleContributer : IBundleContributer
    {
        public void AddScripts(List<BundleDefinition> scriptDefinitions)
        {
            var scripts = new string[]
            {
                "_content/Volo.Abp.AspNetCore.Components.WebAssembly/libs/abp/js/abp.js",
            };

            foreach (var script in scripts)
            {
                scriptDefinitions.AddIfNotContains((item) => item.Source == script, () => new BundleDefinition(script));
            }
        }

        public void AddStyles(List<BundleDefinition> styleDefinitions)
        {
        }
    }
}
