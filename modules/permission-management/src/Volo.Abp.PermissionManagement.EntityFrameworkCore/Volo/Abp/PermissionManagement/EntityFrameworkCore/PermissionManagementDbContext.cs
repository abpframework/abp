using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.PermissionManagement.EntityFrameworkCore
{
    [ConnectionStringName(AbpPermissionManagementDbProperties.ConnectionStringName)]
    public class PermissionManagementDbContext : AbpDbContext<PermissionManagementDbContext>, IPermissionManagementDbContext
    {
        public DbSet<PermissionGrant> PermissionGrants { get; set; }

        public PermissionManagementDbContext(
            DbContextOptions<PermissionManagementDbContext> options,
            IOptions<NamingConventionOptions> namingConventionOptions)
            : base(options, namingConventionOptions)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePermissionManagement();

            builder.ConfigureNamingConvention<PermissionManagementDbContext>(this.NamingConventionOptions);
        }

    }
}
