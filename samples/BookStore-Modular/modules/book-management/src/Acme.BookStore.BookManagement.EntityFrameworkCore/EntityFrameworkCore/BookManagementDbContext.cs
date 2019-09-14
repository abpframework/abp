using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.BookManagement.EntityFrameworkCore
{
    [ConnectionStringName("BookManagement")]
    public class BookManagementDbContext : AbpDbContext<BookManagementDbContext>, IBookManagementDbContext
    {
        public static string TablePrefix { get; set; } = BookManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = BookManagementConsts.DefaultDbSchema;

        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public BookManagementDbContext(DbContextOptions<BookManagementDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBookManagement(options =>
            {
                options.TablePrefix = TablePrefix;
                options.Schema = Schema;
            });
        }
    }
}