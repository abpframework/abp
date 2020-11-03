using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.BlazoriseUI
{
    public class BundleContributer : IBundleContributer
    {
        public void AddScripts(List<BundleDefinition> scriptDefinitions)
        {
            var scripts = new string[]
            {
                "_content/Blazorise/blazorise.js",
                "_content/Blazorise.Bootstrap/blazorise.bootstrap.js"
            };

            foreach (var script in scripts)
            {
                scriptDefinitions.AddIfNotContains((item) => item.Source == script, () => new BundleDefinition(script));
            }
        }

        public void AddStyles(List<BundleDefinition> styleDefinitions)
        {
            var styles = new string[]
            {
                "_content/Blazorise/blazorise.css",
                "_content/Blazorise.Bootstrap/blazorise.bootstrap.css",
                "_content/Blazorise.Snackbar/blazorise.snackbar.css"
            };

            foreach (var style in styles)
            {
                styleDefinitions.AddIfNotContains((item) => item.Source == style, () => new BundleDefinition(style));
            }
        }
    }
}
