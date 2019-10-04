﻿using System.Collections.Generic;
using DashboardDemo.Localization.DashboardDemo;
using DashboardDemo.Pages.widgets;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Identity;
using Volo.Abp.Localization;

namespace DashboardDemo.Widgets
{
    public static class WidgetDefinitionProvider
    {
        public static List<WidgetDefinition> GetDefinitions()
        {
            var userCountWidget = new WidgetDefinition(
                    UserCountWidgetViewComponent.Name,
                    LocalizableString.Create<DashboardDemoResource>(UserCountWidgetViewComponent.DisplayName),
                    typeof(UserCountWidgetViewComponent)
                )
                .SetDefaultDimension(6, 4)
                .AddRequiredPermission(IdentityPermissions.Users.Default);

            var monthlyProfitWidget = new WidgetDefinition(
                    MonthlyProfitWidgetViewComponent.Name,
                    LocalizableString.Create<DashboardDemoResource>(MonthlyProfitWidgetViewComponent.DisplayName),
                    typeof(MonthlyProfitWidgetViewComponent)
                )
                .SetDefaultDimension(6, 4)
                .AddRequiredPermission(IdentityPermissions.Users.Default);

            var roleListWidget = new WidgetDefinition(
                    RoleListWidgetViewComponent.Name,
                    LocalizableString.Create<DashboardDemoResource>(RoleListWidgetViewComponent.DisplayName),
                    typeof(RoleListWidgetViewComponent)
                )
                .SetDefaultDimension(6, 3)
                .AddRequiredPermission(IdentityPermissions.Roles.Default);

            return new List<WidgetDefinition>
            {
                userCountWidget,
                monthlyProfitWidget,
                roleListWidget
            };
        }
    }
}
