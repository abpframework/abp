using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Docs.MongoDB
{
    public class DocsMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public DocsMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix)
            : base(collectionPrefix)
        {
        }
    }
}
