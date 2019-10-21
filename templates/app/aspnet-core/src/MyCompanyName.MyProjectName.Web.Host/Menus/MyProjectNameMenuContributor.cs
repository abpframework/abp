﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MyCompanyName.MyProjectName.Localization;
using MyCompanyName.MyProjectName.MultiTenancy;
using Volo.Abp.Account.Localization;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace MyCompanyName.MyProjectName.Web.Menus
{
    public class MyProjectNameMenuContributor : IMenuContributor
    {
        private readonly IConfiguration _configuration;

        public MyProjectNameMenuContributor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
            else if (context.Menu.Name == StandardMenus.User)
            {
                await ConfigureUserMenuAsync(context);
            }
        }

        private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            if (!MultiTenancyConsts.IsEnabled)
            {
                var administration = context.Menu.GetAdministration();
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<MyProjectNameResource>>();

            context.Menu.Items.Insert(0, new ApplicationMenuItem("MyProjectName.Home", l["Menu:Home"], "/"));

            return Task.CompletedTask;
        }

        private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
        {
            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<MyProjectNameResource>>();
            var accountStringLocalizer = context.ServiceProvider.GetRequiredService<IStringLocalizer<AccountResource>>();

            var identityServerUrl = _configuration["AuthServer:Authority"] ?? "";

            context.Menu.AddItem(new ApplicationMenuItem("Account.Manage", accountStringLocalizer["ManageYourProfile"], $"{identityServerUrl.EnsureEndsWith('/')}Account/Manage", icon: "fa fa-cog", order: 1000, null, "_blank"));
            context.Menu.AddItem(new ApplicationMenuItem("Account.Logout", l["Logout"], url: "/Account/Logout", icon: "fa fa-power-off", order: int.MaxValue - 1000));

            return Task.CompletedTask;
        }
    }
}
