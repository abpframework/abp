using System;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.TestApp.FifthContext;

public interface IFifthDbContextDummyEntityRepository : IBasicRepository<FifthDbContextDummyEntity, Guid>
{

}

public class FifthDbContextDummyEntityRepository :
    EfCoreRepository<IFifthDbContext, FifthDbContextDummyEntity, Guid>,
    IFifthDbContextDummyEntityRepository
{
    public FifthDbContextDummyEntityRepository(IDbContextProvider<IFifthDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
