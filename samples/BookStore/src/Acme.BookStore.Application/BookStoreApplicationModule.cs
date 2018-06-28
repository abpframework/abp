using Microsoft.Extensions.DependencyInjection;
using Acme.BookStore.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Acme.BookStore
{
    [DependsOn(
        typeof(BookStoreDomainModule),
        typeof(AbpIdentityApplicationModule))]
    public class BookStoreApplicationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<BookStorePermissionDefinitionProvider>();
            });

            services.AddAssemblyOf<BookStoreApplicationModule>();
        }
    }
}
