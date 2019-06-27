
using System;
using Volo.Abp;
using Volo.Abp.MongoDB;
using Volo.Docs.Projects;

namespace Volo.Docs.MongoDB
{
    public static class DocsMongoDbContextExtensions
    {
        public static void ConfigureDocs(
            this IMongoModelBuilder builder,
            Action<MongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new DocsMongoModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

            builder.Entity<Project>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Projects";
            });
        }
    }
}

