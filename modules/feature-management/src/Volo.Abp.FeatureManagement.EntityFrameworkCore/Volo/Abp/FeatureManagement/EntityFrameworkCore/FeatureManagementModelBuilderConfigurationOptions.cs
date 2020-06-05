using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.FeatureManagement.EntityFrameworkCore
{
    public class FeatureManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public FeatureManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}