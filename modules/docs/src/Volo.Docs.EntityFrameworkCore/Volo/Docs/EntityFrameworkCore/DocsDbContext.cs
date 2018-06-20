using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Docs.Projects;

namespace Volo.Docs.EntityFrameworkCore
{
    [ConnectionStringName("Docs")]
    public class DocsDbContext: AbpDbContext<DocsDbContext>, IDocsDbContext
    {
        public static string TablePrefix { get; set; } = DocsConsts.DefaultDbTablePrefix;
        public static string Schema { get; set; } = DocsConsts.DefaultDbSchema;

        public DbSet<Project> Projects { get; set; }

        public DocsDbContext(DbContextOptions<DocsDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureDocs(options =>
            {
                options.TablePrefix = TablePrefix;
                options.Schema = Schema;
            });
        }
    }
}
