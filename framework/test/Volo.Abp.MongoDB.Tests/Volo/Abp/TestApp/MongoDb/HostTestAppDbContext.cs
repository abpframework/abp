using MongoDB.Driver;
using Volo.Abp.MongoDB;
using Volo.Abp.MongoDB.TestApp.FifthContext;

namespace Volo.Abp.TestApp.MongoDb;

public class HostTestAppDbContext : AbpMongoDbContext, IFifthDbContext
{
    public IMongoCollection<FifthDbContextDummyEntity> FifthDbContextDummyEntity => Collection<FifthDbContextDummyEntity>();

    public IMongoCollection<FifthDbContextMultiTenantDummyEntity> FifthDbContextMultiTenantDummyEntity => Collection<FifthDbContextMultiTenantDummyEntity>();
}
