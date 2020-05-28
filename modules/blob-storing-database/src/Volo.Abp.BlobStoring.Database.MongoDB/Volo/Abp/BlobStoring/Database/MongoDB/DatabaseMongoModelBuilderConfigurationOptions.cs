using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    public class DatabaseMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public DatabaseMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}