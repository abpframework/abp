using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class AbpAuditLoggingModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public AbpAuditLoggingModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(
                tablePrefix, 
                schema)
        {

        }
    }
}