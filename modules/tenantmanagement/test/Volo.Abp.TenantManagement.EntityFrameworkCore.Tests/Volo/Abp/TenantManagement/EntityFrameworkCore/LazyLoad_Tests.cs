using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.TenantManagement.EntityFrameworkCore
{
    public class LazyLoad_Tests : LazyLoad_Tests<AbpTenantManagementEntityFrameworkCoreTestModule>
    {
        protected override void BeforeAddApplication(IServiceCollection services)
        {
            services.Configure<AbpDbContextOptions>(options =>
            {
                options.PreConfigure<TenantManagementDbContext>(context =>
                {
                    context.DbContextOptions.UseLazyLoadingProxies();
                });
            });
        }
    }
}
