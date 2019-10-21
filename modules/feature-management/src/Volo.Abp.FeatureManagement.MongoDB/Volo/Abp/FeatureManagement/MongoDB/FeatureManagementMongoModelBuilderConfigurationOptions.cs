using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB
{
    public class FeatureManagementMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public FeatureManagementMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {

        }
    }
}