using Volo.Abp.MongoDB;

namespace Volo.Abp.BackgroundJobs.MongoDB
{
    public static class BackgroundJobsMongoDbContextExtensions
    {
        public static void ConfigureBackgroundJobs(
            this IMongoModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<BackgroundJobRecord>(b =>
            {
                b.CollectionName = BackgroundJobsDbProperties.DbTablePrefix + "BackgroundJobs";
            });
        }
    }
}
