using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Ui.Navigation;
using Volo.Abp.Ui.Navigation.Localization.Resource;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.UI.Navigation
{
    [DependsOn(typeof(AbpUiModule))]
    public class AbpUiNavigationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpUiNavigationModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpUiNavigationResource>("en")
                    .AddVirtualJson("/Volo/Abp/Ui/Navigation/Localization/Resource");
            });

            Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new DefaultMenuContributor());
            });
        }
    }
}
