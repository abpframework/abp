using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BackgroundJobs.MongoDB
{
    [ConnectionStringName(BackgroundJobsDbProperties.ConnectionStringName)]
    public class BackgroundJobsMongoDbContext : AbpMongoDbContext, IBackgroundJobsMongoDbContext
    {
        public IMongoCollection<BackgroundJobRecord> BackgroundJobs { get; set; }

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureBackgroundJobs();
        }
    }
}