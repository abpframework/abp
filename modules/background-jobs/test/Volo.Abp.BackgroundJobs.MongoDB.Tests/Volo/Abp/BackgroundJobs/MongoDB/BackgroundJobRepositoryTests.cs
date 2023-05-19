using Xunit;

namespace Volo.Abp.BackgroundJobs.MongoDB;

[Collection((MongoTestCollection.Name))]
public class BackgroundJobRepositoryTests : BackgroundJobRepository_Tests<AbpBackgroundJobsMongoDbTestModule>
{

}
