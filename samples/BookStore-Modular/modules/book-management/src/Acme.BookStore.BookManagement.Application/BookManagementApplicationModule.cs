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
                options.AddMaps<BookManagementApplicationModule>(validate: true);
            });
        }
    }
}
