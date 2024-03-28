using Volo.Abp.Bundling;

namespace MyCompanyName.MyProjectName.Blazor.Host.Client;

public class MyProjectNameBlazorHostBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {

    }

    public void AddStyles(BundleContext context)
    {
        context.Add("main.css", true);
    }
}
