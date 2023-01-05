using MongoDB.Driver;

namespace Volo.Abp.MongoDB.TestApp.FifthContext;

public class FifthDbContext : AbpMongoDbContext, IFifthDbContext
{
    public IMongoCollection<FifthDbContextDummyEntity> FifthDbContextDummyEntity { get; set; }

    public IMongoCollection<FifthDbContextMultiTenantDummyEntity> FifthDbContextMultiTenantDummyEntity { get; set; }
}
