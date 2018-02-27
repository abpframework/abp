using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy.EntityFrameworkCore;

namespace MicroserviceDemo.TenancyService.Db
{
    public class MigrationDbContext : AbpDbContext<MigrationDbContext>
    {
        public MigrationDbContext(DbContextOptions<MigrationDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureMultiTenancy();
        }
    }
}
