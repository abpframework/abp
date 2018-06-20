using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Docs.EntityFrameworkCore;

namespace Volo.DocsTestApp.EntityFrameworkCore
{
    public class DocsTestAppDbContext : AbpDbContext<DocsTestAppDbContext>
    {
        public DocsTestAppDbContext(DbContextOptions<DocsTestAppDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureDocs();
        }
    }
}
