using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.OpenIddict.MongoDB
{
    public class OpenIddictMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public OpenIddictMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}