using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.BookManagement.EntityFrameworkCore
{
    public class MyProjectHttpApiHostMigrationsDbContext : AbpDbContext<MyProjectHttpApiHostMigrationsDbContext>
    {
        public MyProjectHttpApiHostMigrationsDbContext(DbContextOptions<MyProjectHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureBookManagement();
        }
    }
}
