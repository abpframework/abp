using Microsoft.Extensions.DependencyInjection;
using Acme.BookStore.Localization.BookStore;
using Acme.BookStore.Settings;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace Acme.BookStore
{
    [DependsOn(typeof(AbpIdentityDomainModule))]
    public class BookStoreDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BookStoreDomainModule>();
            });

            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<BookStoreResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/BookStore");
            });

            context.Services.Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<BookStoreSettingDefinitionProvider>();
            });

            context.Services.AddAssemblyOf<BookStoreDomainModule>();
        }
    }
}
