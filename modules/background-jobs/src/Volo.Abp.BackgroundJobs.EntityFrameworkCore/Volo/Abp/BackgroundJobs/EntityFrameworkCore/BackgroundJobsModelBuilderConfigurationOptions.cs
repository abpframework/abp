using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.BackgroundJobs.EntityFrameworkCore
{
    public class BackgroundJobsModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public BackgroundJobsModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = BackgroundJobsConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = BackgroundJobsConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}