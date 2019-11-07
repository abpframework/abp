using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class IdentityModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public IdentityModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(
                tablePrefix, 
                schema)
        {

        }
    }
}