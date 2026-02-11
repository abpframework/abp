using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Volo.Abp.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpEfCoreDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder AddAbpDbContextOptionsExtension(this DbContextOptionsBuilder optionsBuilder)
    {
        ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(new AbpDbContextOptionsExtension());
        return optionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> AddAbpDbContextOptionsExtension<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
        where TContext : DbContext
    {
        ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(new AbpDbContextOptionsExtension());
        return optionsBuilder;
    }
}
