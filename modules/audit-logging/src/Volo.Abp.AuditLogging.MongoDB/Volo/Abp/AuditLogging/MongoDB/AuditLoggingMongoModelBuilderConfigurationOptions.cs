using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.AuditLogging.MongoDB
{
    public class AuditLoggingMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public AuditLoggingMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}
