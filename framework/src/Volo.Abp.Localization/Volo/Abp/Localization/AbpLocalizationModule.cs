using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Localization
{
    [DependsOn(typeof(AbpVirtualFileSystemModule))]
    public class AbpLocalizationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            AbpStringLocalizerFactory.Replace(services);

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpLocalizationModule>("Volo.Abp", "Volo/Abp");
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                .Add<AbpValidationResource>("en")
                .AddVirtualJson("/Localization/Resources/AbpValidation");
            });

            services.AddAssemblyOf<AbpLocalizationModule>();
        }
    }
}
