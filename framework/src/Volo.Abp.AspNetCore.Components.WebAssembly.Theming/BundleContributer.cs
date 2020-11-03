using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming
{
    public class BundleContributer : IBundleContributer
    {
        public void AddScripts(List<string> scripts)
        {

        }

        public void AddStyles(List<string> styles)
        {
            styles.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/libs/bootstrap/css/bootstrap.css");
            styles.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/libs/fontawesome/css/all.css");
            styles.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.WebAssembly.Theming/libs/flag-icon/css/flag-icon.css");
        }
    }
}
