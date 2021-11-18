using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.FeatureManagement.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(FeatureManagementDbProperties.ConnectionStringName)]
    public class FeatureManagementDbContext : AbpDbContext<FeatureManagementDbContext>, IFeatureManagementDbContext
    {
        public DbSet<FeatureValue> FeatureValues { get; set; }

        public FeatureManagementDbContext(
            DbContextOptions<FeatureManagementDbContext> options,
            IOptions<NamingConventionOptions> namingConventionOptions)
            : base(options, namingConventionOptions)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureFeatureManagement();

            builder.ConfigureNamingConvention<FeatureManagementDbContext>(this.NamingConventionOptions);
        }


    }
}
