using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.JsTree;

public class JsTreeStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        var options = context
            .ServiceProvider
            .GetRequiredService<IOptions<JsTreeOptions>>()
            .Value;

        if (options.StylePath.IsNullOrEmpty())
        {
            return;
        }

        context.Files.AddIfNotContains(options.StylePath);
    }
}
