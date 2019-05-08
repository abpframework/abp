using Acme.BookStore.Localization.BookStore;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.TenantManagement;
using Volo.Abp.VirtualFileSystem;

namespace Acme.BookStore
{
    [DependsOn(
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpAuditingModule),
        typeof(BackgroundJobsDomainModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpTenantManagementDomainModule),
        typeof(AbpFeatureManagementDomainModule)
        )]
    public class BookStoreDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BookStoreDomainModule>("Acme.BookStore");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<BookStoreResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/BookStore");
            });

            Configure<MultiTenancyOptions>(options =>
            {
                options.IsEnabled = BookStoreConsts.IsMultiTenancyEnabled;
            });
        }
    }
}
