using Volo.Abp.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IAbpDbContextConfigurer
    {
        void Configure(AbpDbContextConfigurationContext context);
    }

    public interface IAbpDbContextConfigurer<TDbContext>
        where TDbContext : AbpDbContext<TDbContext>
    {
        void Configure(AbpDbContextConfigurationContext<TDbContext> context);
    }
}