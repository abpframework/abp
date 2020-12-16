using Volo.Abp.Bundling;

namespace Volo.Abp.BlazoriseUI
{
    public class BlazoriseUIBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context)
        {
            context.Add("_content/Blazorise/blazorise.js");
            context.Add("_content/Blazorise.Bootstrap/blazorise.bootstrap.js");
        }

        public void AddStyles(BundleContext context)
        {
            context.Add("_content/Blazorise/blazorise.css");
            context.Add("_content/Blazorise.Bootstrap/blazorise.bootstrap.css");
            context.Add("_content/Blazorise.Snackbar/blazorise.snackbar.css");
        }
    }
}
