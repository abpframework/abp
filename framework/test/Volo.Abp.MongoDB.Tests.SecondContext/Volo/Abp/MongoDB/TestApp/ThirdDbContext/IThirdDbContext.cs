using MongoDB.Driver;

namespace Volo.Abp.MongoDB.TestApp.ThirdDbContext;

public interface IThirdDbContext : IAbpMongoDbContext
{
    IMongoCollection<ThirdDbContextDummyEntity> DummyEntities { get; }
}
