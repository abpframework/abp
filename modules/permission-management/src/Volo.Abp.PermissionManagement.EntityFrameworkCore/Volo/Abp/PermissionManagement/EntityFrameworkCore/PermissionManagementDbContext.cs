using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.PermissionManagement.EntityFrameworkCore
{
    [ConnectionStringName(AbpPermissionManagementConsts.ConnectionStringName)]
    public class PermissionManagementDbContext : AbpDbContext<PermissionManagementDbContext>, IPermissionManagementDbContext
    {
        public static string TablePrefix { get; set; } = AbpPermissionManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = AbpPermissionManagementConsts.DefaultDbSchema;

        public DbSet<PermissionGrant> PermissionGrants { get; set; }

        public PermissionManagementDbContext(DbContextOptions<PermissionManagementDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePermissionManagement(TablePrefix, Schema);
        }
    }
}
