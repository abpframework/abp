using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Ui.Navigation;
using Volo.Abp.Ui.Navigation.Localization.Resource;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.UI.Navigation
{
    [DependsOn(typeof(UiModule))]
    public class UiNavigationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<UiNavigationModule>();
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
