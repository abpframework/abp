using OrganizationService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OrganizationService.Host.EntityFrameworkCore
{
    public class OrganizationServiceServiceMigrationDbContext : AbpDbContext<OrganizationServiceServiceMigrationDbContext>
    {
        public OrganizationServiceServiceMigrationDbContext(
            DbContextOptions<OrganizationServiceServiceMigrationDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureOrganizationService();
        }
    }
}
