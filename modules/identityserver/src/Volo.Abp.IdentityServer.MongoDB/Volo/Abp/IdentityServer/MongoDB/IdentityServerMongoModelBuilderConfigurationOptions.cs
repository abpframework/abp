using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class IdentityServerMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public IdentityServerMongoModelBuilderConfigurationOptions([NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}
