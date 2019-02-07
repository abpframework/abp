using Acme.BookStore.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Acme.BookStore
{
    [DependsOn(
        typeof(BookStoreDomainModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule))]
    public class BookStoreApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<BookStorePermissionDefinitionProvider>();
            });

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<BookStoreApplicationAutoMapperProfile>();
            });
        }
    }
}
