using Microsoft.Extensions.DependencyInjection;
using Acme.BookStore.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Acme.BookStore
{
    [DependsOn(
        typeof(BookStoreDomainModule),
        typeof(AbpIdentityApplicationModule))]
    public class BookStoreApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<BookStorePermissionDefinitionProvider>();
            });

            context.Services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<BookStoreApplicationAutoMapperProfile>();
            });

            context.Services.AddAssemblyOf<BookStoreApplicationModule>();
        }
    }
}
