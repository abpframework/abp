using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace Volo.Abp.OpenIddict.Tokens;

public class EfCoreOpenIddictTokenRepository : EfCoreRepository<IOpenIddictDbContext, OpenIddictToken, Guid>, IOpenIddictTokenRepository
{
    public EfCoreOpenIddictTokenRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public async Task DeleteManyByApplicationIdAsync(Guid applicationId, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var tokens = await (await GetDbSetAsync())
            .Where(x => x.ApplicationId == applicationId)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, autoSave, cancellationToken);
    }

    public async Task DeleteManyByAuthorizationIdAsync(Guid authorizationId, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var tokens = await (await GetDbSetAsync())
            .Where(x => x.AuthorizationId == authorizationId)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, autoSave, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.Subject == subject && x.ApplicationId == client).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.Subject == subject && x.ApplicationId == client && x.Status == status).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, string type, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.Subject == subject && x.ApplicationId == client && x.Status == status && x.Type == type).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.ApplicationId == applicationId).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(Guid authorizationId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.AuthorizationId == authorizationId).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictToken> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictToken> FindByReferenceIdAsync(string referenceId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(x => x.ReferenceId == referenceId, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).Where(x => x.Subject == subject).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .OrderBy(x => x.Id)
            .SkipIf<OpenIddictToken, IQueryable<OpenIddictToken>>(offset.HasValue, offset.Value)
            .TakeIf<OpenIddictToken, IQueryable<OpenIddictToken>>(count.HasValue, count.Value)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<List<OpenIddictToken>> GetPruneListAsync(DateTime date, int count, CancellationToken cancellationToken = default)
    {
        return await (from token in await GetQueryableAsync()
            join authorization in (await GetDbContextAsync()).Set<OpenIddictAuthorization>().AsQueryable()
                on token.AuthorizationId equals authorization.Id into ta
            from a in ta
            where token.CreationDate < date
            where (token.Status != OpenIddictConstants.Statuses.Inactive &&
                   token.Status != OpenIddictConstants.Statuses.Valid) ||
                  (a != null && a.Status != OpenIddictConstants.Statuses.Valid) ||
                  token.ExpirationDate < DateTime.UtcNow
            orderby token.Id
            select token).Take(count)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
