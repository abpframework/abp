using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BackgroundJobs.MongoDB
{
    [ConnectionStringName("AbpBackgroundJobs")]
    public interface IBackgroundJobsMongoDbContext : IAbpMongoDbContext
    {
         IMongoCollection<BackgroundJobRecord> BackgroundJobs { get; }
    }
}
