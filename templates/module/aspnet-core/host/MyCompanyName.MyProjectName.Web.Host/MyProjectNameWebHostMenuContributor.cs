﻿using System;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace MyCompanyName.MyProjectName
{
    public class MyProjectNameWebHostMenuContributor : IMenuContributor
    {
        private readonly IConfiguration _configuration;

        public MyProjectNameWebHostMenuContributor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.User)
            {
                AddLogoutItemToMenu(context);
            }

            return Task.CompletedTask;
        }

        private void AddLogoutItemToMenu(MenuConfigurationContext context)
        {
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
            var l = context.GetLocalizer<MyProjectNameResource>();

            if (currentUser.IsAuthenticated)
            {
                context.Menu.Items.Add(new ApplicationMenuItem(
                    "Account.Manage",
                    l["ManageYourProfile"],
                    $"{_configuration["AuthServer:Authority"].EnsureEndsWith('/')}Account/Manage",
                    icon: "fa fa-cog",
                    order: int.MaxValue - 1001,
                    null,
                    "_blank")
                    );


                context.Menu.Items.Add(new ApplicationMenuItem(
                    "Account.Logout",
                    l["Logout"],
                    "~/Account/Logout",
                    "fas fa-power-off",
                    order: int.MaxValue - 1000
                ));
            }
        }
    }
}
