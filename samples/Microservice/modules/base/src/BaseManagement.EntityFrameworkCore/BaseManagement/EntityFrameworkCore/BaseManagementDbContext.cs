using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace BaseManagement.EntityFrameworkCore
{
    [ConnectionStringName("BaseManagement")]
    public class BaseManagementDbContext : AbpDbContext<BaseManagementDbContext>, IBaseManagementDbContext
    {
        public static string TablePrefix { get; set; } = BaseManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = BaseManagementConsts.DefaultDbSchema;

        public DbSet<BaseType> BaseTypes { get; set; }
        public DbSet<BaseItem> BaseItems { get; set; }

        public BaseManagementDbContext(DbContextOptions<BaseManagementDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBaseManagement(options =>
            {
                options.TablePrefix = TablePrefix;
                options.Schema = Schema;
            });
        }
    }
}