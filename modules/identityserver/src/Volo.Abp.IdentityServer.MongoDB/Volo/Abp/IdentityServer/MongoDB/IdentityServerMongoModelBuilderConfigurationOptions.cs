using System;
using System.Collections.Generic;
using System.Text;
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
