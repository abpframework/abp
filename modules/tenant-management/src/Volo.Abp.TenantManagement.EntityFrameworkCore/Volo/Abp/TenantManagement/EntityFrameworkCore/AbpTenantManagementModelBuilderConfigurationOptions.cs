using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.TenantManagement.EntityFrameworkCore
{
    public class AbpTenantManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public AbpTenantManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}