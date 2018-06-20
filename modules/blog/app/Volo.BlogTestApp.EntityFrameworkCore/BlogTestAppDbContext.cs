using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Blog.EntityFrameworkCore;

namespace Volo.BlogTestApp.EntityFrameworkCore
{
    public class BlogTestAppDbContext : AbpDbContext<BlogTestAppDbContext>
    {
        public BlogTestAppDbContext(DbContextOptions<BlogTestAppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureBlog();
        }
    }
}
