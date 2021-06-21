using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Localization.Resource;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.UI.Navigation
{
    [DependsOn(typeof(AbpUiModule), typeof(AbpAuthorizationModule))]
    public class AbpUiNavigationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpUiNavigationModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpUiNavigationResource>("en")
                    .AddVirtualJson("/Volo/Abp/Ui/Navigation/Localization/Resource");
            });

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new DefaultMenuContributor());
            });
        }
    }
}
