using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB
{
    public class FeatureManagementMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public FeatureManagementMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {

        }
    }
}