using Microsoft.EntityFrameworkCore;
using ProductManagement.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ProductService.Host.EntityFrameworkCore
{
    public class ProductServiceMigrationDbContext : AbpDbContext<ProductServiceMigrationDbContext>
    {
        public ProductServiceMigrationDbContext(
            DbContextOptions<ProductServiceMigrationDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureProductManagement();
        }
    }
}
