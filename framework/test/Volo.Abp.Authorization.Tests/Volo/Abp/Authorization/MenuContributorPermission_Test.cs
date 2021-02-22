using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.UI.Navigation;
using Xunit;

namespace Volo.Abp.Authorization
{
    public class MenuContributorPermission_Test : AuthorizationTestBase
    {
        public static readonly List<string> GrantedPermissions = new List<string>();
        private readonly IMenuManager _menuManager;

        public MenuContributorPermission_Test()
        {
            _menuManager = GetRequiredService<IMenuManager>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new TestMenuContributor1());
                options.MenuContributors.Add(new TestMenuContributor2());
            });

            base.AfterAddApplication(services);
        }

        [Fact]
        public async Task Should_Check_MenuContributor_PreCheckPermissions()
        {
            await _menuManager.GetAsync(StandardMenus.Main);

            GrantedPermissions.Count.ShouldBe(2);
            GrantedPermissions.ShouldContain("MenuPermission1");
            GrantedPermissions.ShouldContain("MenuPermission4");
        }

        class TestMenuContributor1 : MenuContributorBase
        {
            public TestMenuContributor1()
            {
                PreCheckPermissions.Add("MenuPermission1");
                PreCheckPermissions.Add("MenuPermission2");
            }

            public override Task ConfigureMenuAsync(MenuConfigurationContext context)
            {
                MenuContributorPermission_Test.GrantedPermissions.AddRange(GrantedPermissions);

                return Task.CompletedTask;
            }
        }

        class TestMenuContributor2 : MenuContributorBase
        {
            public TestMenuContributor2()
            {
                PreCheckPermissions.Add("MenuPermission3");
                PreCheckPermissions.Add("MenuPermission4");
            }

            public override Task ConfigureMenuAsync(MenuConfigurationContext context)
            {
                MenuContributorPermission_Test.GrantedPermissions.AddRange(GrantedPermissions);

                return Task.CompletedTask;
            }
        }
    }
}
