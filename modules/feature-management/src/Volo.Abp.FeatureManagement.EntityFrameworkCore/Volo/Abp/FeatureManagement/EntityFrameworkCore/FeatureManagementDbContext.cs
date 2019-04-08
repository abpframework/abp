using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.FeatureManagement.EntityFrameworkCore
{
    [ConnectionStringName("AbpFeatureManagement")]
    public class FeatureManagementDbContext : AbpDbContext<FeatureManagementDbContext>, IFeatureManagementDbContext
    {
        public static string TablePrefix { get; set; } = FeatureManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = FeatureManagementConsts.DefaultDbSchema;

        public DbSet<FeatureValue> FeatureValues { get; set; }

        public FeatureManagementDbContext(DbContextOptions<FeatureManagementDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureFeatureManagement(options =>
            {
                options.TablePrefix = TablePrefix;
                options.Schema = Schema;
            });
        }
    }
}