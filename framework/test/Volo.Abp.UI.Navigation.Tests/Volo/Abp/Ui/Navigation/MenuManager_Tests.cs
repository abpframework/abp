using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.UI.Navigation
{
    public class MenuManager_Tests : AbpIntegratedTest<AbpUiNavigationTestModule>
    {
        private readonly IMenuManager _menuManager;

        public MenuManager_Tests()
        {
            _menuManager = ServiceProvider.GetRequiredService<IMenuManager>();
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            var claims = new List<Claim>() {
                new Claim(AbpClaimTypes.UserId, "1fcf46b2-28c3-48d0-8bac-fa53268a2775"),
            };

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var principalAccessor = Substitute.For<ICurrentPrincipalAccessor>();
            principalAccessor.Principal.Returns(ci => claimsPrincipal);
            Thread.CurrentPrincipal = claimsPrincipal;
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
            mainMenu.Items[1].Items[3].Name.ShouldBe("Administration.SubMenu1"); //No need permission.
            // Administration.SubMenu1.1 and Administration.SubMenu1.2 are removed because of don't have permissions.
        }

        [Fact]
        public async Task GetMainMenuAsync_ShouldMergeMultipleMenus()
        {
            var mainMenu = await _menuManager.GetMainMenuAsync();
            
            mainMenu.Name.ShouldBe(StandardMenus.Main);
            
            mainMenu.Items.Count.ShouldBe(3);
            
            mainMenu.Items.ShouldContain(x => x.Name == "Products");
            mainMenu.Items.ShouldContain(x => x.Name == "Dashboard");
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

                administration.AddItem(new ApplicationMenuItem("Administration.UserManagement", "User Management", url: "/admin/users").RequirePermissions("Administration.UserManagement"));
                administration.AddItem(new ApplicationMenuItem("Administration.RoleManagement", "Role Management", url: "/admin/roles").RequirePermissions("Administration.RoleManagement"));

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

                context.Menu.Items.Insert(0, new ApplicationMenuItem("Dashboard", "Dashboard", url: "/dashboard").RequirePermissions("Dashboard"));

                var administration = context.Menu.GetAdministration();

                administration.AddItem(new ApplicationMenuItem("Administration.DashboardSettings", "Dashboard Settings", url: "/admin/settings/dashboard").RequirePermissions("Administration.DashboardSettings"));

                administration.AddItem(
                    new ApplicationMenuItem("Administration.SubMenu1", "Sub menu 1", url: "/submenu1")
                        .AddItem(new ApplicationMenuItem("Administration.SubMenu1.1", "Sub menu 1.1", url: "/submenu1/submenu1_1").RequirePermissions("Administration.SubMenu1.1"))
                        .AddItem(new ApplicationMenuItem("Administration.SubMenu1.2", "Sub menu 1.2", url: "/submenu1/submenu1_2").RequirePermissions("Administration.SubMenu1.2"))
                );

                return Task.CompletedTask;
            }
        }
    
        /* Adds menu items:
         * - Products
         *   - AspNetZero
         *   - ABP
         */
        public class TestMenuContributor3 : IMenuContributor
        {
            public const string MenuName = "MenuThree";
            public Task ConfigureMenuAsync(MenuConfigurationContext context)
            {
                if (context.Menu.Name != MenuName)
                {
                    return Task.CompletedTask;
                }

                var products = new ApplicationMenuItem("Products", "Products", "/products");
                context.Menu.Items.Add(products);

                products.AddItem(new ApplicationMenuItem("AspNetZero", "AspNetZero", url: "/products/aspnetzero"));

                products.AddItem(new ApplicationMenuItem("ABP", "ABP", url: "/products/abp"));

                return Task.CompletedTask;
            }
        }
    }
}
