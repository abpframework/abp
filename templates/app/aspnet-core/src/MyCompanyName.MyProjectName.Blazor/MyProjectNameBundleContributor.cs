using Volo.Abp.Bundling;

namespace MyCompanyName.MyProjectName.Blazor
{
    public class MyProjectNameBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context, BundleParameterDictionary parameters)
        {
        }

        public void AddStyles(BundleContext context, BundleParameterDictionary parameters)
        {
            context.Add("main.css");
        }
    }
}
