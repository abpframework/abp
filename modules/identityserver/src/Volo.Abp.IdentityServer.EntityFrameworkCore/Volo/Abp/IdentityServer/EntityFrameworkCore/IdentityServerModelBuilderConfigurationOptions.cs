using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore
{
    public class IdentityServerModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public IdentityServerModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}