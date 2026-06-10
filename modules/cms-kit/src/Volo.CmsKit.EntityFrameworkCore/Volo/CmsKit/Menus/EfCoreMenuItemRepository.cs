using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Menus;

public class EfCoreMenuItemRepository : EfCoreRepository<ICmsKitDbContext, MenuItem, Guid>, IMenuItemRepository
{
    public EfCoreMenuItemRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<int> GetHighestMenuOrderAsync(Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(parentId.HasValue, x => x.ParentId == parentId)
            .OrderByDescending(x => x.Order)
            .Select(x => x.Order)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<MenuItem>> GetOrderedListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Order)
            .ThenBy(x => x.CreationTime)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
