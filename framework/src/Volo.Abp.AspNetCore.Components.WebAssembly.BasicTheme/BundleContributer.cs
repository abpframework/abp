using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme
{
    public class BundleContributer : IBundleContributer
    {
        public void AddScripts(List<BundleDefinition> scriptDefinitions)
        {

        }

        public void AddStyles(List<BundleDefinition> styleDefinitions)
        {
            var styles = new string[]
            {
                "_content/Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme/libs/abp/css/theme.css",
            };

            foreach (var style in styles)
            {
                styleDefinitions.AddIfNotContains((item) => item.Source == style, () => new BundleDefinition(style));
            }
        }
    }
}
