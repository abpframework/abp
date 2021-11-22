using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Core;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;

[DependsOn(typeof(CoreScriptContributor))]
public class JQueryScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/jquery/jquery.js");
        context.Files.AddIfNotContains("/libs/abp/jquery/abp.jquery.js");
    }
}
