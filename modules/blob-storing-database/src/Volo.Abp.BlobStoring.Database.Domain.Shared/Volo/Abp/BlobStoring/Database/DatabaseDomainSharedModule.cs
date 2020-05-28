using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Volo.Abp.BlobStoring.Database.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.BlobStoring.Database
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class DatabaseDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<DatabaseDomainSharedModule>("Volo.Abp.BlobStoring.Database");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<DatabaseResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Database");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Database", typeof(DatabaseResource));
            });
        }
    }
}
