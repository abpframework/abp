using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MyCompanyName.MyProjectName.Blazor.WebApp.Tiered.Client;

/* Add your global styles/scripts here.
 * See https://docs.abp.io/en/abp/latest/UI/Blazor/Global-Scripts-Styles to learn how to use it
 */
public class MyProjectNameBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("main.css", true));
    }
}
