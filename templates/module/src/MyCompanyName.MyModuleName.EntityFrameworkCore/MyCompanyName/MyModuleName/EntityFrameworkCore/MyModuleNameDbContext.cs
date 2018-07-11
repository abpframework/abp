using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace MyCompanyName.MyModuleName.EntityFrameworkCore
{
    [ConnectionStringName("MyModuleName")]
    public class MyModuleNameDbContext : AbpDbContext<MyModuleNameDbContext>, IMyModuleNameDbContext
    {
        public static string TablePrefix { get; set; } = MyModuleNameConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = MyModuleNameConsts.DefaultDbSchema;

        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public MyModuleNameDbContext(DbContextOptions<MyModuleNameDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureMyModuleName(options =>
            {
                options.TablePrefix = TablePrefix;
                options.Schema = Schema;
            });
        }
    }
}