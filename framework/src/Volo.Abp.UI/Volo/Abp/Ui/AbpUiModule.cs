using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.UI
{
    [DependsOn(
        typeof(AbpLocalizationModule)
    )]
    public class AbpUiModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpUiModule>();
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<AbpUiResource>("en").AddVirtualJson("/Localization/Resources/AbpUi");
            });
            
            services.AddAssemblyOf<AbpUiModule>();
        }
    }
}
