using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class IdentityModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public IdentityModelBuilderConfigurationOptions()
            : base(
                AbpIdentityDbProperties.DbTablePrefix,
                AbpIdentityDbProperties.DbSchema)
        {

        }
    }
}
