using Volo.Abp.Bundling;

namespace MyCompanyName.MyProjectName.Blazor
{
    public class MyProjectNameBundleContributer : IBundleContributer
    {
        public void AddScripts(BundleContext context)
        {
        }

        public void AddStyles(BundleContext context)
        {
            context.Add("main.css");
        }
    }
}
