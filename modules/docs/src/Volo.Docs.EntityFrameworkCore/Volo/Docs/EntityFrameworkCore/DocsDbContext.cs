using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Docs.Projects;

namespace Volo.Docs.EntityFrameworkCore
{
    [ConnectionStringName(DocsDbProperties.ConnectionStringName)]
    public class DocsDbContext: AbpDbContext<DocsDbContext>, IDocsDbContext
    {
        public DbSet<Project> Projects { get; set; }

        public DocsDbContext(DbContextOptions<DocsDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureDocs();
        }
    }
}
