using BaseManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BaseManagement.Host.EntityFrameworkCore
{
    public class BaseManagementServiceMigrationDbContext : AbpDbContext<BaseManagementServiceMigrationDbContext>
    {
        public BaseManagementServiceMigrationDbContext(
            DbContextOptions<BaseManagementServiceMigrationDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureBaseManagement();
        }
    }
}
