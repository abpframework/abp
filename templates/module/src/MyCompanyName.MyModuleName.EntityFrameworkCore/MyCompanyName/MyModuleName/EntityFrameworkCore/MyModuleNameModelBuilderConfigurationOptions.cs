using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyCompanyName.MyModuleName.EntityFrameworkCore
{
    public class MyModuleNameModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public MyModuleNameModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = MyModuleNameConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = MyModuleNameConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}