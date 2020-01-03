using Localization.Resources.AbpUi;
using Acme.BookStore.BookManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.BookStore.BookManagement
{
    [DependsOn(
        typeof(BookManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class BookManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(BookManagementHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<BookManagementResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
