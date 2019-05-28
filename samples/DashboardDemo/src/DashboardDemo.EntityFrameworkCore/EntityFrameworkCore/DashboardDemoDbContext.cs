using Microsoft.EntityFrameworkCore;
using DashboardDemo.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Users.EntityFrameworkCore;

namespace DashboardDemo.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class DashboardDemoDbContext : AbpDbContext<DashboardDemoDbContext>
    {
        public DbSet<AppUser> Users { get; set; }

        public DashboardDemoDbContext(DbContextOptions<DashboardDemoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure the shared tables (with included modules) here */

            builder.Entity<AppUser>(b =>
            {
                b.ToTable("AbpUsers"); //Sharing the same table "AbpUsers" with the IdentityUser

                b.ConfigureFullAudited();
                b.ConfigureExtraProperties();
                b.ConfigureConcurrencyStamp();
                b.ConfigureAbpUser();

                //Moved customization to a method so we can share it with the DashboardDemoMigrationsDbContext class
                b.ConfigureCustomUserProperties();
            });

            /* Configure your own tables/entities inside the ConfigureDashboardDemo method */

            builder.ConfigureDashboardDemo();
        }
    }
}
