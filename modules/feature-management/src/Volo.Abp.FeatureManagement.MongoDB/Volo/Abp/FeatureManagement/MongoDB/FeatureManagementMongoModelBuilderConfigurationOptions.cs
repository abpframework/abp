using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Abp.FeatureManagement.MongoDB
{
    public class FeatureManagementMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public FeatureManagementMongoModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = FeatureManagementConsts.DefaultDbTablePrefix)
            : base(tablePrefix)
        {
        }
    }
}