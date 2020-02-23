using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.UI.Navigation
{
    public class MenuManager_Tests : AbpIntegratedTest<MenuManager_Tests.TestModule>
    {
        private readonly IMenuManager _menuManager;

        public MenuManager_Tests()
        {
            _menuManager = ServiceProvider.GetRequiredService<IMenuManager>();
        }

        [Fact]
        public async Task Should_Get_Menu()
        {
            var mainMenu = await _menuManager.GetAsync(StandardMenus.Main);

            mainMenu.Name.ShouldBe(StandardMenus.Main);
            mainMenu.DisplayName.ShouldBe("Main Menu");
            mainMenu.Items.Count.ShouldBe(2);
            mainMenu.Items[0].Name.ShouldBe("Dashboard");
            mainMenu.Items[1].Name.ShouldBe(DefaultMenuNames.Application.Main.Administration);
            mainMenu.Items[1].Items[0].Name.ShouldBe("Administration.UserManagement");
            mainMenu.Items[1].Items[1].Name.ShouldBe("Administration.RoleManagement");
            mainMenu.Items[1].Items[2].Name.ShouldBe("Administration.DashboardSettings");
            mainMenu.Items[1].Items[3].Name.ShouldBe("Administration.SubMenu1");
            mainMenu.Items[1].Items[3].Items[0].Name.ShouldBe("Administration.SubMenu1.1");
            mainMenu.Items[1].Items[3].Items[1].Name.ShouldBe("Administration.SubMenu1.2");
        }

        [DependsOn(typeof(AbpUiNavigationModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                Configure<AbpNavigationOptions>(options =>
                {
                    options.MenuContributors.Add(new TestMenuContributor1());
                    options.MenuContributors.Add(new TestMenuContributor2());
                });
            }
        }

        /* Adds menu items:
         * - Administration
         *   - User Management
         *   - Role Management
         */
        public class TestMenuContributor1 : IMenuContributor
        {
            public Task ConfigureMenuAsync(MenuConfigurationContext context)
            {
                if (context.Menu.Name != StandardMenus.Main)
                {
                    return Task.CompletedTask;
                }

                context.Menu.DisplayName = "Main Menu";

                var administration = context.Menu.GetAdministration();

                administration.AddItem(new ApplicationMenuItem("Administration.UserManagement", "User Management", url: "/admin/users"));
                administration.AddItem(new ApplicationMenuItem("Administration.RoleManagement", "Role Management", url: "/admin/roles"));

                return Task.CompletedTask;
            }
        }

        /* Adds menu items:
         * - Dashboard
         * - Administration
         *   - Dashboard Settings
         */
        public class TestMenuContributor2 : IMenuContributor
        {
            public Task ConfigureMenuAsync(MenuConfigurationContext context)
            {
                if (context.Menu.Name != StandardMenus.Main)
                {
                    return Task.CompletedTask;
                }

                context.Menu.Items.Insert(0, new ApplicationMenuItem("Dashboard", "Dashboard", url: "/dashboard"));

                var administration = context.Menu.GetAdministration();

                administration.AddItem(new ApplicationMenuItem("Administration.DashboardSettings", "Dashboard Settings", url: "/admin/settings/dashboard"));

                administration.AddItem(
                    new ApplicationMenuItem("Administration.SubMenu1", "Sub menu 1")
                        .AddItem(new ApplicationMenuItem("Administration.SubMenu1.1", "Sub menu 1.1", url: "/submenu1/submenu1_1"))
                        .AddItem(new ApplicationMenuItem("Administration.SubMenu1.2", "Sub menu 1.2", url: "/submenu1/submenu1_2"))
                );

                return Task.CompletedTask;
            }
        }
    }
}
