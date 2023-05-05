using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Contributors;

public class AbpPreloadStyleLoadScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/abp/aspnetcore-mvc-ui-theme-shared/csp/preload-style-load.js");
    }
}