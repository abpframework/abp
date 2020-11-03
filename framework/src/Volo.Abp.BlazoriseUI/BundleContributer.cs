using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.BlazoriseUI
{
    public class BundleContributer : IBundleContributer
    {
        public void AddScripts(List<string> scripts)
        {
            scripts.AddIfNotContains("_content/Blazorise/blazorise.js");
            scripts.AddIfNotContains("_content/Blazorise.Bootstrap/blazorise.bootstrap.js");
        }

        public void AddStyles(List<string> styles)
        {
            styles.AddIfNotContains("_content/Blazorise/blazorise.css");
            styles.AddIfNotContains("_content/Blazorise.Bootstrap/blazorise.bootstrap.css");
            styles.AddIfNotContains("_content/Blazorise.Snackbar/blazorise.snackbar.css");
        }
    }
}
