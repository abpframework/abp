using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    [ConnectionStringName("MyProjectName")]
    public class MyProjectNameDbContext : AbpDbContext<MyProjectNameDbContext>, IMyProjectNameDbContext
    {
        public static string TablePrefix { get; set; } = MyProjectNameConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = MyProjectNameConsts.DefaultDbSchema;

        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public MyProjectNameDbContext(DbContextOptions<MyProjectNameDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureMyProjectName(options =>
            {
                options.TablePrefix = TablePrefix;
                options.Schema = Schema;
            });
        }
    }
}