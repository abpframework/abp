using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Blogging.MongoDB
{
    public class BloggingMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public BloggingMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}
