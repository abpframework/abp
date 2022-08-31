using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Moment;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.BootstrapDaterangepicker;

[DependsOn(typeof(JQueryScriptContributor))]
[DependsOn(typeof(MomentScriptContributor))]
public class BootstrapDaterangepickerScriptContributor : BundleContributor
{
    public const string PackageName = "bootstrap-daterangepicker";

    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/bootstrap-daterangepicker/daterangepicker.js");
    }
}
