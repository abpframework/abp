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

    public virtual async Task DeleteManyByApplicationIdAsync(Guid applicationId, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var tokens = await (await GetDbSetAsync())
            .Where(x => x.ApplicationId == applicationId)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, autoSave, cancellationToken);
    }

    public virtual async Task DeleteManyByAuthorizationIdAsync(Guid authorizationId, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var tokens = await (await GetDbSetAsync())
            .Where(x => x.AuthorizationId == authorizationId)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, autoSave, GetCancellationToken(cancellationToken));
    }

    public virtual async Task DeleteManyByAuthorizationIdsAsync(Guid[] authorizationIds, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var tokens = await (await GetDbSetAsync())
            .Where(x => x.AuthorizationId != null && authorizationIds.Contains(x.AuthorizationId.Value))
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, autoSave, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid? client, string status, string type, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .WhereIf(!subject.IsNullOrWhiteSpace(), x => x.Subject == subject)
            .WhereIf(client.HasValue, x => x.ApplicationId == client)
            .WhereIf(!status.IsNullOrWhiteSpace(), x => x.Status == status)
            .WhereIf(!type.IsNullOrWhiteSpace(), x => x.Type == type)
            .ToListAsync(GetCancellationToken(cancellationToken));
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
            .SkipIf<OpenIddictToken, IQueryable<OpenIddictToken>>(offset.HasValue, offset)
            .TakeIf<OpenIddictToken, IQueryable<OpenIddictToken>>(count.HasValue, count)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> PruneAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        return await (from token in await GetQueryableAsync()
                join authorization in (await GetDbContextAsync()).Set<OpenIddictAuthorization>()
                    on token.AuthorizationId equals authorization.Id into tokenAuthorizations
                from tokenAuthorization in tokenAuthorizations.DefaultIfEmpty()
                where token.CreationDate < date
                where (token.Status != OpenIddictConstants.Statuses.Inactive && token.Status != OpenIddictConstants.Statuses.Valid) ||
                      (tokenAuthorization != null && tokenAuthorization.Status != OpenIddictConstants.Statuses.Valid) ||
                      token.ExpirationDate < DateTime.UtcNow
                select token)
            .ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async ValueTask<long> RevokeAsync(string subject, Guid? applicationId, string status, string type, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryableAsync())
            .WhereIf(!subject.IsNullOrWhiteSpace(), x => x.Subject == subject)
            .WhereIf(applicationId.HasValue, x => x.ApplicationId == applicationId)
            .WhereIf(!status.IsNullOrWhiteSpace(), x => x.Status == status)
            .WhereIf(!type.IsNullOrWhiteSpace(), x => x.Type == type);

        return await query.ExecuteUpdateAsync(entity => entity.SetProperty(
            authorization => authorization.Status, OpenIddictConstants.Statuses.Revoked), cancellationToken);
    }

    public virtual async ValueTask<long> RevokeByAuthorizationIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await (from token in await GetQueryableAsync() where token.AuthorizationId == id select token)
            .ExecuteUpdateAsync(
                entity => entity.SetProperty(token => token.Status, OpenIddictConstants.Statuses.Revoked),
                GetCancellationToken(cancellationToken));
    }

    public virtual async ValueTask<long> RevokeByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        return await (from token in await GetQueryableAsync()
            where token.ApplicationId == applicationId
            select token).ExecuteUpdateAsync(entity => entity.SetProperty(
            token => token.Status, OpenIddictConstants.Statuses.Revoked), cancellationToken);
    }

    public virtual async ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        return await (from token in await GetQueryableAsync()
            where token.Subject == subject
            select token).ExecuteUpdateAsync(entity => entity.SetProperty(
            token => token.Status, OpenIddictConstants.Statuses.Revoked), cancellationToken);
    }
}
