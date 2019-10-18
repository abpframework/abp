using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore
{
    public class IdentityServerModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public EfCoreDatabaseProvider? DatabaseProvider { get; set; }

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