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

namespace Volo.Abp.OpenIddict.Applications;

public class EfCoreOpenIddictApplicationRepository : EfCoreRepository<IOpenIddictDbContext, OpenIddictApplication, Guid>, IOpenIddictApplicationRepository
{
    public EfCoreOpenIddictApplicationRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<OpenIddictApplication>> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.ClientId.Contains(filter))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(OpenIddictApplication.CreationTime) + " desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.ClientId.Contains(filter))
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictApplication> FindByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .FirstOrDefaultAsync(x => x.ClientId == clientId, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictApplication>> FindByPostLogoutRedirectUriAsync(string address, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.PostLogoutRedirectUris.Contains(address)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictApplication>> FindByRedirectUriAsync(string address, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.RedirectUris.Contains(address)).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictApplication>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .SkipIf<OpenIddictApplication, IQueryable<OpenIddictApplication>>(offset.HasValue, offset)
            .TakeIf<OpenIddictApplication, IQueryable<OpenIddictApplication>>(count.HasValue, count)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
