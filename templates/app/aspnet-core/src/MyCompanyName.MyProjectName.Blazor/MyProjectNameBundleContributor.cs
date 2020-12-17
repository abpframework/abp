using Volo.Abp.Bundling;

namespace MyCompanyName.MyProjectName.Blazor
{
    public class MyProjectNameBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context)
        {

        }

        public void AddStyles(BundleContext context)
        {
            context.Add("main.css", true);
        }
    }
}