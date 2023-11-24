using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB;

public class MongoIdentitySessionRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentitySession, Guid>, IIdentitySessionRepository
{
    public MongoIdentitySessionRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }
}
