using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Acme.BookStore.BookManagement.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.VirtualFileSystem;

namespace Acme.BookStore.BookManagement
{
    [DependsOn(
        typeof(AbpLocalizationModule)
    )]
    public class BookManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BookManagementDomainSharedModule>("Acme.BookStore.BookManagement");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<BookManagementResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/BookManagement");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("BookManagement", typeof(BookManagementResource));
            });
        }
    }
}
