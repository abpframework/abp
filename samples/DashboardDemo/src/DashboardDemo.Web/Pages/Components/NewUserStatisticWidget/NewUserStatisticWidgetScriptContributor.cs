using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardDemo.Web.Bundles;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace DashboardDemo.Web.Pages.Components.NewUserStatisticWidget
{
    [DependsOn(typeof(ChartjsScriptContributor))]
    public class NewUserStatisticWidgetScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/Pages/Components/NewUserStatisticWidget/Default.js");
        }
    }
}
