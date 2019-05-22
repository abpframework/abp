using System.Collections.Generic;
using DashboardDemo.Localization.DashboardDemo;
using DashboardDemo.Pages.widgets;
using DashboardDemo.Widgets;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace DashboardDemo.Dashboards
{
    public static class DashboardDefinitionProvider
    {
        public static List<DashboardDefinition> GetDefinitions()
        {
            var myDashboard = new DashboardDefinition(
                DashboardNames.MyDashboard,
                LocalizableString.Create<DashboardDemoResource>("MyDashboard")
                )
                .WithWidget(DemoStatisticsViewComponent.WidgetName)
                .WithWidget(MyWidgetViewComponent.WidgetName);

            return new List<DashboardDefinition>
            {
                myDashboard
            };
        }
    }

    [DependsOn(
        typeof(AbpBasicDashboardScriptContributor),
        typeof(MyWidgetViewComponentScriptBundleContributor),
        typeof(DemoStatisticsScriptContributor)
        )]
    public class MyDashboardScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {

        }
    }
}
