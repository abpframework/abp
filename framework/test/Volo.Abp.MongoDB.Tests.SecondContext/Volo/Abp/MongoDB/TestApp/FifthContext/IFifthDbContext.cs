using MongoDB.Driver;

namespace Volo.Abp.MongoDB.TestApp.FifthContext;

public interface IFifthDbContext : IAbpMongoDbContext
{
    IMongoCollection<FifthDbContextDummyEntity> FifthDbContextDummyEntity { get; }

    IMongoCollection<FifthDbContextMultiTenantDummyEntity> FifthDbContextMultiTenantDummyEntity { get; }
}
