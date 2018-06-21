using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;

namespace Volo.Blogging.EntityFrameworkCore
{
    [ConnectionStringName("Blogging")]
    public class BloggingDbContext : AbpDbContext<BloggingDbContext>, IBloggingDbContext
    {
        public static string TablePrefix { get; set; } = BloggingConsts.DefaultDbTablePrefix;
        public static string Schema { get; set; } = BloggingConsts.DefaultDbSchema;

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }
        
        public BloggingDbContext(DbContextOptions<BloggingDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBlogging(options =>
            {
                options.TablePrefix = TablePrefix;
                options.Schema = Schema;
            });
        }
    }
}