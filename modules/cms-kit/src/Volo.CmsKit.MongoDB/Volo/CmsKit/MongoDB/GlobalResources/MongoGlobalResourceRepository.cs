using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.GlobalResources;

namespace Volo.CmsKit.MongoDB.GlobalResources;

public class MongoGlobalResourceRepository: MongoDbRepository<ICmsKitMongoDbContext, GlobalResource, Guid>, IGlobalResourceRepository
{
    public MongoGlobalResourceRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public Task<GlobalResource> FindByNameAsync(string name,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(name, nameof(name));
        return FindAsync(x => x.Name == name, cancellationToken: GetCancellationToken(cancellationToken));
    }
}