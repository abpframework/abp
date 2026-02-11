using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Volo.Abp.OpenIddict.Scopes;

public class EfCoreOpenIddictScopeRepository : EfCoreRepository<IOpenIddictDbContext, OpenIddictScope, Guid>, IOpenIddictScopeRepository
{
    public EfCoreOpenIddictScopeRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<OpenIddictScope>> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => 
                x.Name.Contains(filter) ||
                x.DisplayName.Contains(filter) ||
                x.Description.Contains(filter))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(OpenIddictScope.CreationTime) + " desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => 
                x.Name.Contains(filter) ||
                x.DisplayName.Contains(filter) ||
                x.Description.Contains(filter))
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }
    
    public virtual async Task<OpenIddictScope> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictScope> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(x => x.Name == name, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictScope>> FindByNamesAsync(string[] names, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => names.Contains(x.Name)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictScope>> FindByResourceAsync(string resource, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.Resources.Contains(resource)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictScope>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .OrderBy(x => x.Id)
            .SkipIf<OpenIddictScope, IQueryable<OpenIddictScope>>(offset.HasValue, offset)
            .TakeIf<OpenIddictScope, IQueryable<OpenIddictScope>>(count.HasValue, count)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
