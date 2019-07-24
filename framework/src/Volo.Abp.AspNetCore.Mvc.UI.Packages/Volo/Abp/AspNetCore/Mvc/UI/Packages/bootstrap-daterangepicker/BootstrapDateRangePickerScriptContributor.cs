using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Moment;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.bootstrap_daterangepicker
{
    [DependsOn(typeof(MomentScriptContributor))]
    public class BootstrapDateRangePickerScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/bootstrap-daterangepicker/daterangepicker.js");
        }
    }
}
