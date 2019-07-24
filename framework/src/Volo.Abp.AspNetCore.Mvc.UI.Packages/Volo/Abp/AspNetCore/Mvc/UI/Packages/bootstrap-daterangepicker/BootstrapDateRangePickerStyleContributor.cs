using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.bootstrap_daterangepicker
{
    public class BootstrapDateRangePickerStyleContributor :  BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/bootstrap-daterangepicker/daterangepicker.css");
        }
    }
}
