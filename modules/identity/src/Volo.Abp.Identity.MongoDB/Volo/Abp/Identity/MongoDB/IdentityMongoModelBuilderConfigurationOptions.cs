using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class IdentityMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public IdentityMongoModelBuilderConfigurationOptions()
            : base(AbpIdentityConsts.DefaultDbTablePrefix)
        {
        }
    }
}