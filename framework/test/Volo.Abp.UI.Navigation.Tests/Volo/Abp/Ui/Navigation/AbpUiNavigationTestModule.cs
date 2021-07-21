using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.UI.Navigation
{
    [DependsOn(typeof(AbpUiNavigationModule))]
    [DependsOn(typeof(AbpAuthorizationModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    public class AbpUiNavigationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new MenuManager_Tests.TestMenuContributor1());
                options.MenuContributors.Add(new MenuManager_Tests.TestMenuContributor2());
                options.MenuContributors.Add(new MenuManager_Tests.TestMenuContributor3());
                
                options.MainMenuNames.Add(MenuManager_Tests.TestMenuContributor3.MenuName);
            });
        }
    }
}
