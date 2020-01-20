using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    public class MyProjectNameHttpApiHostMigrationsDbContext : AbpDbContext<MyProjectNameHttpApiHostMigrationsDbContext>
    {
        public MyProjectNameHttpApiHostMigrationsDbContext(DbContextOptions<MyProjectNameHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureMyProjectName();
        }
    }
}
