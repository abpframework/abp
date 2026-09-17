using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OpenIddict.Abstractions;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.MongoDB;

namespace Volo.Abp.OpenIddict.Tokens;

public class MongoOpenIddictTokenRepository : MongoDbRepository<OpenIddictMongoDbContext, OpenIddictToken, Guid>, IOpenIddictTokenRepository
{
    public MongoOpenIddictTokenRepository(IMongoDbContextProvider<OpenIddictMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task DeleteManyByApplicationIdAsync(Guid applicationId, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var tokens = await (await GetQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.ApplicationId == applicationId)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, autoSave, cancellationToken);
    }

    public virtual async Task DeleteManyByAuthorizationIdAsync(Guid authorizationId, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var tokens = await (await GetQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.AuthorizationId == authorizationId)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, autoSave, GetCancellationToken(cancellationToken));
    }

    public virtual async Task DeleteManyByAuthorizationIdsAsync(Guid[] authorizationIds, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var tokens = await (await GetQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.AuthorizationId != null && authorizationIds.AsEnumerable().Contains(x.AuthorizationId.Value))
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, autoSave, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid? client, string status, string type, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .WhereIf(!subject.IsNullOrWhiteSpace(), x => x.Subject == subject)
            .WhereIf(client.HasValue, x => x.ApplicationId == client)
            .WhereIf(!status.IsNullOrWhiteSpace(), x => x.Status == status)
            .WhereIf(!type.IsNullOrWhiteSpace(), x => x.Type == type)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.ApplicationId == applicationId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(Guid authorizationId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.AuthorizationId == authorizationId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictToken> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(GetCancellationToken(cancellationToken))).FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictToken> FindByReferenceIdAsync(string referenceId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(GetCancellationToken(cancellationToken))).FirstOrDefaultAsync(x => x.ReferenceId == referenceId, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.Subject == subject)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        return  await (await GetQueryableAsync(GetCancellationToken(cancellationToken)))
            .OrderBy(x => x.Id)
            .SkipIf<OpenIddictToken, IQueryable<OpenIddictToken>>(offset.HasValue, offset)
            .TakeIf<OpenIddictToken, IQueryable<OpenIddictToken>>(count.HasValue, count)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> PruneAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var authorizationIds = await (await GetQueryableAsync<OpenIddictAuthorization>(cancellationToken))
            .Where(x => x.Status != OpenIddictConstants.Statuses.Valid)
            .Select(x => x.Id)
            .ToListAsync(GetCancellationToken(cancellationToken));

        var tokens = await (await GetQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.CreationDate < date)
            .Where(x => (x.Status != OpenIddictConstants.Statuses.Inactive &&
                         x.Status != OpenIddictConstants.Statuses.Valid) ||
                        authorizationIds.Contains(x.Id) ||
                        x.ExpirationDate < DateTime.UtcNow)
            .OrderBy(x => x.Id)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, cancellationToken: cancellationToken);

        return tokens.Count;
    }

    public virtual async ValueTask<long> RevokeAsync(string subject, Guid? applicationId, string status, string type, CancellationToken cancellationToken = default)
    {
        var filter = Builders<OpenIddictToken>.Filter.Empty;

        if (!string.IsNullOrEmpty(subject))
        {
            filter &= Builders<OpenIddictToken>.Filter.Where(authorization => authorization.Subject == subject);
        }

        if (applicationId.HasValue)
        {
            filter &= Builders<OpenIddictToken>.Filter.Where(authorization => authorization.ApplicationId == applicationId);
        }

        if (!string.IsNullOrEmpty(status))
        {
            filter &= Builders<OpenIddictToken>.Filter.Where(authorization => authorization.Status == status);
        }

        if (!string.IsNullOrEmpty(type))
        {
            filter &= Builders<OpenIddictToken>.Filter.Where(authorization => authorization.Type == type);
        }

        return (await (await GetCollectionAsync(cancellationToken)).UpdateManyAsync(
            filter           : filter,
            update           : Builders<OpenIddictToken>.Update.Set(authorization => authorization.Status, OpenIddictConstants.Statuses.Revoked),
            options          : null,
            cancellationToken: cancellationToken)).MatchedCount;
    }

    public virtual async ValueTask<long> RevokeByAuthorizationIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return (await (await GetCollectionAsync(GetCancellationToken(cancellationToken))).UpdateManyAsync(
            filter           : token => token.AuthorizationId == id,
            update           : Builders<OpenIddictToken>.Update.Set(token => token.Status, OpenIddictConstants.Statuses.Revoked),
            options          : null,
            cancellationToken: GetCancellationToken(cancellationToken))).MatchedCount;
    }

    public virtual async ValueTask<long> RevokeByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        return (await (await GetCollectionAsync(cancellationToken)).UpdateManyAsync(
            filter           : token => token.ApplicationId == applicationId,
            update           : Builders<OpenIddictToken>.Update.Set(token => token.Status, OpenIddictConstants.Statuses.Revoked),
            options          : null,
            cancellationToken: cancellationToken)).MatchedCount;
    }

    public virtual async ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        return (await (await GetCollectionAsync(cancellationToken)).UpdateManyAsync(
            filter           : token => token.Subject == subject,
            update           : Builders<OpenIddictToken>.Update.Set(token => token.Status, OpenIddictConstants.Statuses.Revoked),
            options          : null,
            cancellationToken: cancellationToken)).MatchedCount;
    }
}
