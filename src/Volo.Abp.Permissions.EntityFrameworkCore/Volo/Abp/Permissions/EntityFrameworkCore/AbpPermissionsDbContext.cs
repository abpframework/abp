using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Permissions.EntityFrameworkCore
{
    [ConnectionStringName("AbpPermissions")]
    public class AbpPermissionsDbContext : AbpDbContext<AbpPermissionsDbContext>, IAbpPermissionsDbContext
    {
        public static string TablePrefix { get; set; } = AbpPermissionConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = AbpPermissionConsts.DefaultDbSchema;

        public DbSet<PermissionGrant> PermissionGrants { get; set; }

        public AbpPermissionsDbContext(DbContextOptions<AbpPermissionsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureAbpPermissions(TablePrefix, Schema);
        }
    }
}
