using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrganizationService.EntityFrameworkCore
{
    [ConnectionStringName("OrganizationService")]
    public class OrganizationServiceDbContext : AbpDbContext<OrganizationServiceDbContext>, IOrganizationServiceDbContext
    {
        public static string TablePrefix { get; set; } = OrganizationServiceConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = OrganizationServiceConsts.DefaultDbSchema;

        public DbSet<AbpOrganization> AbpOrganizations { get; set; }

        public OrganizationServiceDbContext(DbContextOptions<OrganizationServiceDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureOrganizationService(options =>
            {
                options.TablePrefix = TablePrefix;
                options.Schema = Schema;
            });
        }
    }
}