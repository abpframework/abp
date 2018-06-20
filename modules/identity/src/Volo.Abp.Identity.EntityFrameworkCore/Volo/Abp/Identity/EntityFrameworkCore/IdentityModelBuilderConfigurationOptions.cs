using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class IdentityModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public IdentityModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = AbpIdentityConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = AbpIdentityConsts.DefaultDbSchema)
            : base(
                tablePrefix, 
                schema)
        {

        }
    }
}