using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.BloggingTestApp.EntityFrameworkCore
{
    public class BloggingTestAppDbContext : AbpDbContext<BloggingTestAppDbContext>
    {
        public BloggingTestAppDbContext(DbContextOptions<BloggingTestAppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureBlogging();
        }
    }
}
