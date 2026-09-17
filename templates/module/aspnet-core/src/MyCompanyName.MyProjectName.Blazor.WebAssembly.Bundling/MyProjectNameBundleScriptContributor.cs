using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MyCompanyName.MyProjectName.Blazor.WebAssembly.Bundling;

public class MyProjectNameBundleStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        // Add your css files here
    }
}
