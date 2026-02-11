using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MyCompanyName.MyProjectName.Blazor.Client;

/* Add your global styles/scripts here.
 * See https://abp.io/docs/latest/framework/ui/blazor/global-scripts-styles to learn how to use it
 */
public class MyProjectNameBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("main.css", true));
    }
}
