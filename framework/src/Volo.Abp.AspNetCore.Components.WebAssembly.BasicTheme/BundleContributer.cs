using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme
{
    public class BundleContributer : IBundleContributer
    {
        public void AddScripts(List<string> scripts)
        {

        }

        public void AddStyles(List<string> styles)
        {
            styles.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme/libs/abp/css/theme.css");
        }
    }
}
