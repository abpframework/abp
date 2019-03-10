using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Abp.FeatureManagement.EntityFrameworkCore
{
    public class FeatureManagementModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public FeatureManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = FeatureManagementConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = FeatureManagementConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}