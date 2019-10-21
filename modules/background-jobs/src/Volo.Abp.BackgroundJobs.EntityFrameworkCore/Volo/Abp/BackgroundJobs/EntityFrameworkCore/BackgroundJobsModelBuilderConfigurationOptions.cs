using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.BackgroundJobs.EntityFrameworkCore
{
    public class BackgroundJobsModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public BackgroundJobsModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}