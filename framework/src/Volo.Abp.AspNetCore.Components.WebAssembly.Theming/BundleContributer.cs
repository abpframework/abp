using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming
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
                "_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/libs/bootstrap/css/bootstrap.min.css",
                "_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/libs/fontawesome/css/all.css",
                "_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/libs/flag-icon/css/flag-icon.css"
            };

            foreach (var style in styles)
            {
                styleDefinitions.AddIfNotContains((item) => item.Source == style, () => new BundleDefinition(style));
            }
        }
    }
}
