using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    public class MyProjectNameModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public MyProjectNameModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = MyProjectNameConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = MyProjectNameConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}