using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Blog.EntityFrameworkCore
{
    [ConnectionStringName("Blog")]
    public class BlogDbContext : AbpDbContext<BlogDbContext>, IBlogDbContext
    {
        public static string TablePrefix { get; set; } = BlogConsts.DefaultDbTablePrefix;
        public static string Schema { get; set; } = BlogConsts.DefaultDbSchema;

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBlog(options =>
            {
                options.TablePrefix = TablePrefix;
                options.Schema = Schema;
            });
        }
    }
}