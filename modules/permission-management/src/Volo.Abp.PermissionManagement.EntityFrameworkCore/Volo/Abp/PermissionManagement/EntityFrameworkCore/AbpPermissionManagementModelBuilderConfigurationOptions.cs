using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.PermissionManagement.EntityFrameworkCore
{
    public class AbpPermissionManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public AbpPermissionManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}