using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.Localization.MyProjectName;
using MyCompanyName.MyProjectName.Settings;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(AbpIdentityDomainModule),
        typeof(AbpAuditingModule),
        typeof(BackgroundJobsDomainModule),
        typeof(AbpAuditLoggingDomainModule)
        )]
    public class MyProjectNameDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyProjectNameDomainModule>();
            });

            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MyProjectNameResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/MyProjectName");
            });

            context.Services.Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<MyProjectNameSettingDefinitionProvider>();
            });
        }
    }
}
