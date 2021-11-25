using Volo.Abp;
using Volo.Abp.MongoDB;
using Volo.Docs.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs.MongoDB
{
    public static class DocsMongoDbContextExtensions
    {
        public static void ConfigureDocs(
            this IMongoModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Project>(b =>
            {
                b.CollectionName = DocsDbProperties.DbTablePrefix + "Projects";
            });

            builder.Entity<Document>(b =>
            {
                b.CollectionName = DocsDbProperties.DbTablePrefix + "DocumentS";
            });
        }
    }
}

