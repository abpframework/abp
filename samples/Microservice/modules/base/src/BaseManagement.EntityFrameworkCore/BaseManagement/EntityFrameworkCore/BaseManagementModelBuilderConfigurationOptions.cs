using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace BaseManagement.EntityFrameworkCore
{
    public class BaseManagementModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public BaseManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = BaseManagementConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = BaseManagementConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}