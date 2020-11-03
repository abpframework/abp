using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.Http.Client.IdentityModel.WebAssembly
{
    public class BundleContributer : IBundleContributer
    {
        public void AddScripts(List<BundleDefinition> scriptDefinitions)
        {
            var scripts = new string[]
            {
                "_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js",
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
