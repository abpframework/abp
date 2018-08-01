using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BackgroundJobs.MongoDB
{
    [ConnectionStringName("AbpBackgroundJobs")]
    public class BackgroundJobsMongoDbContext : AbpMongoDbContext, IBackgroundJobsMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = BackgroundJobsConsts.DefaultDbTablePrefix;

        public IMongoCollection<BackgroundJobRecord> BackgroundJobs { get; set; }

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureBackgroundJobs(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}