using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BackgroundJobs.MongoDB
{
    public class BackgroundJobsMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public BackgroundJobsMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}