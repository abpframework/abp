using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ProductManagement.EntityFrameworkCore
{
    public class ProductManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ProductManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = ProductManagementConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = ProductManagementConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}