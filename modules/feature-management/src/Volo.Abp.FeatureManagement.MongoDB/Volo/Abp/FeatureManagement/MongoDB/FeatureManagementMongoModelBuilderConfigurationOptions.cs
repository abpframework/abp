using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB
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