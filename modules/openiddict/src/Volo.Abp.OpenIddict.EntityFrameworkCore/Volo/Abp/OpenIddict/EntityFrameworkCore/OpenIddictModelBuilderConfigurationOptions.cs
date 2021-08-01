using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore
{
    public class OpenIddictModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public OpenIddictModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}