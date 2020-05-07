using Volo.Abp.Autofac;
using Volo.Abp.Emailing.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Emailing
{
    [DependsOn(
        typeof(AbpEmailingModule),
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule))]
    public class AbpEmailingTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpEmailingTestModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpEmailingTestResource>()
                    .AddVirtualJson("/Volo/Abp/Emailing/Localization");
            });

        }
    }
}