using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    public class MyProjectNameModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public MyProjectNameModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}