using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Emailing.Templates.Virtual;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Emailing
{
    [DependsOn(
        typeof(AbpSettingsModule),
        typeof(AbpVirtualFileSystemModule),
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpLocalizationModule)
        )]
    public class AbpEmailingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpEmailingModule>();
            });

            Configure<BackgroundJobOptions>(options =>
            {
                options.AddJob<BackgroundEmailSendingJob>();
            });

            Configure<EmailTemplateOptions>(options =>
            {
                options.Templates
                    .Add(
                        new EmailTemplateDefinition(StandardEmailTemplates.DefaultLayout, isLayout: true, layout: null)
                            .SetVirtualFilePath("/Volo/Abp/Emailing/Templates/DefaultLayout.html")
                    ).Add(
                        new EmailTemplateDefinition(StandardEmailTemplates.SimpleMessage)
                            .SetVirtualFilePath("/Volo/Abp/Emailing/Templates/SimpleMessageTemplate.html")
                    );
            });
        }
    }
}
