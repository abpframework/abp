using Volo.Abp.Emailing.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Emailing
{
    [DependsOn(
        typeof(EmailingModule),
        typeof(TestBaseModule))]
    public class EmailingTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<EmailingTestModule>();
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