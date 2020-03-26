using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace DashboardDemo.EntityFrameworkCore
{
    public static class DashboardDemoDbContextModelCreatingExtensions
    {
        public static void ConfigureDashboardDemo(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(DashboardDemoConsts.DbTablePrefix + "YourEntities", DashboardDemoConsts.DbSchema);

            //    //...
            //});
        }
    }
}