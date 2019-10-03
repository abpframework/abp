using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Acme.BookStore.BookManagement
{
    [DependsOn(
        typeof(BookManagementDomainModule),
        typeof(BookManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class BookManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                /* Using `true` for the `validate` parameter to
                 * validate the profile on application startup.
                 * See http://docs.automapper.org/en/stable/Configuration-validation.html for more info
                 * about the configuration validation. */
                options.AddProfile<BookManagementApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}
