using Microsoft.EntityFrameworkCore;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.EntityFrameworkCore.Modeling
{
    public static class AbpModelBuilderObjectExtensions
    {
        public static void TryConfigureObjectExtensions<TDbContext>(this ModelBuilder modelBuilder)
            where TDbContext : DbContext
        {
            ObjectExtensionManager.Instance.ConfigureEfCoreDbContext<TDbContext>(modelBuilder);
        }
    }
}
