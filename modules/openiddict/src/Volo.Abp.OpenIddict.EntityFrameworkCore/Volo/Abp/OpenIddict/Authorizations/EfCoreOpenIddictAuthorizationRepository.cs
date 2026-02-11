using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.Authorizations;

public class EfCoreOpenIddictAuthorizationRepository : EfCoreRepository<IOpenIddictDbContext, OpenIddictAuthorization, Guid>, IOpenIddictAuthorizationRepository
{
    public EfCoreOpenIddictAuthorizationRepository(IDbContextProvider<IOpenIddictDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid? client, string status, string type, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!subject.IsNullOrWhiteSpace(), x => x.Subject == subject)
            .WhereIf(client.HasValue, x => x.ApplicationId == client)
            .WhereIf(!status.IsNullOrWhiteSpace(), x => x.Status == status)
            .WhereIf(!type.IsNullOrWhiteSpace(), x => x.Type == type)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.ApplicationId == applicationId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictAuthorization> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Subject == subject)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        var query = (await GetDbSetAsync())
            .OrderBy(authorization => authorization.Id!)
            .AsTracking();

        if (offset.HasValue)
        {
            query = query.Skip(offset.Value);
        }

        if (count.HasValue)
        {
            query = query.Take(count.Value);
        }

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> PruneAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var authorizations = await (from authorization in (await GetQueryableAsync())
            join token in (await GetDbContextAsync()).Set<OpenIddictToken>()
                on authorization.Id equals token.AuthorizationId into authorizationTokens
            from authorizationToken in authorizationTokens.DefaultIfEmpty()
            where authorization.CreationDate < date
            where authorization.Status != OpenIddictConstants.Statuses.Valid ||
                  (authorization.Type == OpenIddictConstants.AuthorizationTypes.AdHoc && authorizationToken == null)
            select authorization.Id).ToListAsync(cancellationToken);

        var count = await (from token in (await GetDbContextAsync()).Set<OpenIddictToken>()
                where token.AuthorizationId != null && authorizations.Contains(token.AuthorizationId.Value)
                select token)
            .ExecuteDeleteAsync(GetCancellationToken(cancellationToken));

        return count + await (await GetDbSetAsync()).Where(x => authorizations.Contains(x.Id)).ExecuteDeleteAsync(cancellationToken);
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

    public virtual async ValueTask<long> RevokeByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        return await (from authorization in await GetQueryableAsync()
            where authorization.ApplicationId == applicationId
            select authorization).ExecuteUpdateAsync(entity => entity.SetProperty(
            authorization => authorization.Status, OpenIddictConstants.Statuses.Revoked), cancellationToken);
    }

    public virtual async ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        return await (from authorization in await GetQueryableAsync()
            where authorization.Subject == subject
            select authorization).ExecuteUpdateAsync(entity => entity.SetProperty(
            authorization => authorization.Status, OpenIddictConstants.Statuses.Revoked), cancellationToken);
    }
}
