using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class IdentityMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public IdentityMongoModelBuilderConfigurationOptions([NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}