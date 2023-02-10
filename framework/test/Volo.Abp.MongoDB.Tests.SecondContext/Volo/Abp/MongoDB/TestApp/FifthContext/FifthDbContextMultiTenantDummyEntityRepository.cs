using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.MongoDB;

namespace Volo.Abp.MongoDB.TestApp.FifthContext;

public interface IFifthDbContextMultiTenantDummyEntityRepository : IBasicRepository<FifthDbContextMultiTenantDummyEntity, Guid>
{
    Task<IAbpMongoDbContext> GetDbContextAsync();
}

public class FifthDbContextMultiTenantDummyEntityRepository :
    MongoDbRepository<IFifthDbContext, FifthDbContextMultiTenantDummyEntity, Guid>,
    IFifthDbContextMultiTenantDummyEntityRepository
{
    public FifthDbContextMultiTenantDummyEntityRepository(IMongoDbContextProvider<IFifthDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<IAbpMongoDbContext> GetDbContextAsync()
    {
        return await base.GetDbContextAsync();
    }
}
