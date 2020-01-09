using JetBrains.Annotations;
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class AbpDbContextOptionsCosmosExtensions
    {
        public static void UseCosmos(
            [NotNull] this AbpDbContextOptions options,
            [CanBeNull] Action<CosmosDbContextOptionsBuilder> cosmosOptionsAction = null)
        {
            options.Configure(context =>
            {
                context.UseCosmos(cosmosOptionsAction);
            });
        }

        public static void UseCosmos<TDbContext>(
            [NotNull] this AbpDbContextOptions options,
            [CanBeNull] Action<CosmosDbContextOptionsBuilder> cosmosOptionsAction = null)
            where TDbContext : AbpDbContext<TDbContext>
        {
            options.Configure<TDbContext>(context =>
            {
                context.UseCosmos(cosmosOptionsAction);
            });
        }
    }
}
