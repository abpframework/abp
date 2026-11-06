using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.RobotsTxts;

namespace Volo.CmsKit.MongoDB.RobotsTxts;

public class MongoRobotsTxtRepository : MongoDbRepository<ICmsKitMongoDbContext, RobotsTxt, Guid>, IRobotsTxtRepository
{
    public MongoRobotsTxtRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual Task<RobotsTxt> FindAsync(CancellationToken cancellationToken = default)
    {
        return FindAsync(x => true, cancellationToken: GetCancellationToken(cancellationToken));
    }
}
