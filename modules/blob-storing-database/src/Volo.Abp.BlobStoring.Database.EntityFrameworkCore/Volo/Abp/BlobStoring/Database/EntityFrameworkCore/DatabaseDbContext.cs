using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    [ConnectionStringName(DatabaseDbProperties.ConnectionStringName)]
    public class DatabaseDbContext : AbpDbContext<DatabaseDbContext>, IDatabaseDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureDatabase();
        }
    }
}