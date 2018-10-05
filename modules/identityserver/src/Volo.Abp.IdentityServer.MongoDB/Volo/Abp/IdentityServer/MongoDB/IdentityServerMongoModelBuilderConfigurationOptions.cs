using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class IdentityServerMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public IdentityServerMongoModelBuilderConfigurationOptions()
            : base(AbpIdentityServerConsts.DefaultDbTablePrefix)
        {
        }
    }
}
