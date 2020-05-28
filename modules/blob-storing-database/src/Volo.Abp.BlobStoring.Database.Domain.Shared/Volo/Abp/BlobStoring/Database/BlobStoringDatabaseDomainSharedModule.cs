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
    public class BlobStoringDatabaseDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BlobStoringDatabaseDomainSharedModule>("Volo.Abp.BlobStoring.Database");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<BlobStoringDatabaseResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/BlobStoringDatabase");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("BlobStoringDatabase", typeof(BlobStoringDatabaseResource));
            });
        }
    }
}
