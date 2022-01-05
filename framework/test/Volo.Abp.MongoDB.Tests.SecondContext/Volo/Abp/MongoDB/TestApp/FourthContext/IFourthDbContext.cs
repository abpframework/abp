using MongoDB.Driver;
using Volo.Abp.EntityFrameworkCore.TestApp.FourthContext;

namespace Volo.Abp.MongoDB.TestApp.FourthContext;

public interface IFourthDbContext : IAbpMongoDbContext
{
    IMongoCollection<FourthDbContextDummyEntity> FourthDummyEntities { get; }
}
