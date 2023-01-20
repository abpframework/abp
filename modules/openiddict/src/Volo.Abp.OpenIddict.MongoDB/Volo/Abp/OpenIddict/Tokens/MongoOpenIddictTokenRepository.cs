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
        var tokens = await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.ApplicationId == applicationId)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, autoSave, cancellationToken);
    }

    public virtual async Task DeleteManyByAuthorizationIdAsync(Guid authorizationId, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var tokens = await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.AuthorizationId == authorizationId)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, autoSave, GetCancellationToken(cancellationToken));
    }

    public virtual async Task DeleteManyByAuthorizationIdsAsync(Guid[] authorizationIds, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var tokens = await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.AuthorizationId != null && authorizationIds.Contains(x.AuthorizationId.Value))
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, autoSave, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, CancellationToken cancellationToken = default)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(cancellationToken)), x => x.Subject == subject && x.ApplicationId == client)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, CancellationToken cancellationToken = default)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))), x => x.Subject == subject && x.ApplicationId == client && x.Status == status)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, string type, CancellationToken cancellationToken = default)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))), x => x.Subject == subject && x.ApplicationId == client && x.Status == status && x.Type == type)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))), x => x.ApplicationId == applicationId)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(Guid authorizationId, CancellationToken cancellationToken = default)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))), x => x.AuthorizationId == authorizationId)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictToken> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))).FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictToken> FindByReferenceIdAsync(string referenceId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))).FirstOrDefaultAsync(x => x.ReferenceId == referenceId, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))), x => x.Subject == subject)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        return await Queryable.OrderBy((await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))), x => x.Id)
            .SkipIf<OpenIddictToken, IQueryable<OpenIddictToken>>(offset.HasValue, offset)
            .TakeIf<OpenIddictToken, IQueryable<OpenIddictToken>>(count.HasValue, count)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task PruneAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var authorizationIds = await (await GetMongoQueryableAsync<OpenIddictAuthorization>(cancellationToken))
            .Where(x => x.Status != OpenIddictConstants.Statuses.Valid)
            .Select(x => x.Id)
            .ToListAsync(GetCancellationToken(cancellationToken));

        var tokens = await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .Where(x => x.CreationDate < date)
            .Where(x => (x.Status != OpenIddictConstants.Statuses.Inactive &&
                         x.Status != OpenIddictConstants.Statuses.Valid) ||
                        authorizationIds.Contains(x.Id) ||
                        x.ExpirationDate < DateTime.UtcNow)
            .OrderBy(x => x.Id)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(tokens, cancellationToken: cancellationToken);
    }
}
