using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.MongoDB.Menus;

public class MongoMenuItemRepository : MongoDbRepository<ICmsKitMongoDbContext, MenuItem, Guid>, IMenuItemRepository
{
    public MongoMenuItemRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<int> GetHighestMenuOrderAsync(Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        return await (await GetQueryableAsync(cancellationToken))
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .OrderByDescending(x => x.Order)
            .Select(x => x.Order)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<List<MenuItem>> GetOrderedListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Order)
            .ThenBy(x => x.CreationTime)
            .ToListAsync(cancellationToken);
    }
}
