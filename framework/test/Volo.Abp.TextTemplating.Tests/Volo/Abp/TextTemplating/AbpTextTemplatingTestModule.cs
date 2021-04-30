using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating
{
    [DependsOn(
        typeof(AbpTextTemplatingAbstractionsModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAutofacModule),
        typeof(AbpLocalizationModule)
    )]
    public class AbpTextTemplatingTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpTextTemplatingTestModule>("Volo.Abp.TextTemplating");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<TestLocalizationSource>("en")
                    .AddVirtualJson("/Localization");
            });
        }
    }
}
