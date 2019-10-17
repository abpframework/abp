using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class IdentityModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
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