using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{
    public class QuartzDatabaseModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public QuartzDatabaseModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}