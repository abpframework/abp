using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.TextTemplating;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Emailing
{
    [DependsOn(
        typeof(AbpSettingsModule),
        typeof(AbpVirtualFileSystemModule),
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpLocalizationModule),
        typeof(AbpTextTemplatingModule)
        )]
    public class AbpEmailingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpEmailingModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<EmailingResource>("en")
                    .AddVirtualJson("/Volo/Abp/Emailing/Localization");
            });

            Configure<AbpBackgroundJobOptions>(options =>
            {
                options.AddJob<BackgroundEmailSendingJob>();
            });
        }
    }
}
