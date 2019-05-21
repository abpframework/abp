using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardDemo.Localization.DashboardDemo;
using DashboardDemo.Pages.widgets;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Localization;

namespace DashboardDemo
{
    public class WidgetDefinitionProvider : IWidgetDefinitionProvider
    {
        public List<WidgetDefinition> GetDefinitions()
        {
            var widgets = new List<WidgetDefinition>();

            var myWidget = new WidgetDefinition("MyWidget", typeof(MyWidgetViewComponentModel),
                new LocalizableString(typeof(DashboardDemoResource), "MyWidgett"));

            widgets.Add(myWidget);

            return widgets;
        }
    }
}
