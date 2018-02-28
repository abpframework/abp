using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace MicroserviceDemo.AuthServer.Db
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

            modelBuilder.ConfigureIdentityServer();
        }
    }
}
