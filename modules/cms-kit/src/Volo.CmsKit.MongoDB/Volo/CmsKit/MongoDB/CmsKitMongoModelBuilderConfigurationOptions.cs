using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.CmsKit.MongoDB
{
    public class CmsKitMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public CmsKitMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}