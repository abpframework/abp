using Volo.Abp.Bundling;

namespace Volo.Abp.BlazoriseUI
{
    public class BundleContributer : BaseBundleContributer
    {
        public override string[] GetStyles()
        {
            return new string[]
            {
                "_content/Blazorise/blazorise.css",
                "_content/Blazorise.Bootstrap/blazorise.bootstrap.css",
                "_content/Blazorise.Snackbar/blazorise.snackbar.css"
            };
        }

        public override string[] GetScripts()
        {
            return new string[]
            {
                "_content/Blazorise/blazorise.js",
                "_content/Blazorise.Bootstrap/blazorise.bootstrap.js"
            };
        }
    }
}
