using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.RobotsTxts;

public class EfCoreRobotsTxtRepository : EfCoreRepository<ICmsKitDbContext, RobotsTxt, Guid>, IRobotsTxtRepository
{
    public EfCoreRobotsTxtRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual Task<RobotsTxt> FindAsync(CancellationToken cancellationToken = default)
    {
        return FindAsync(x => true, cancellationToken: GetCancellationToken(cancellationToken));
    }
}
