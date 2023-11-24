using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore;

public class EfCoreIdentitySessionRepository : EfCoreRepository<IIdentityDbContext, IdentitySession, Guid>, IIdentitySessionRepository
{
    public EfCoreIdentitySessionRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

}
