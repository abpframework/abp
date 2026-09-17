using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OpenIddict.Abstractions;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.OpenIddict.MongoDB;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.Authorizations;

public class MongoOpenIddictAuthorizationRepository : MongoDbRepository<OpenIddictMongoDbContext, OpenIddictAuthorization, Guid>, IOpenIddictAuthorizationRepository
{
    protected IMongoDbRepositoryFilterer<OpenIddictToken, Guid> TokenDbRepositoryFilterer { get; }

    public MongoOpenIddictAuthorizationRepository(
        IMongoDbContextProvider<OpenIddictMongoDbContext> dbContextProvider,
        IMongoDbRepositoryFilterer<OpenIddictToken, Guid> tokenDbRepositoryFilterer)
        : base(dbContextProvider)
    {
        TokenDbRepositoryFilterer = tokenDbRepositoryFilterer;
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid? client, string status, string type, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(cancellationToken))
            .WhereIf(!subject.IsNullOrWhiteSpace(), x => x.Subject == subject)
            .WhereIf(client.HasValue, x => x.ApplicationId == client)
            .WhereIf(!status.IsNullOrWhiteSpace(), x => x.Status == status)
            .WhereIf(!type.IsNullOrWhiteSpace(), x => x.Type == type)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(GetCancellationToken(cancellationToken))).Where(x => x.ApplicationId == applicationId).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictAuthorization> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(GetCancellationToken(cancellationToken))).FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(GetCancellationToken(cancellationToken))).Where(x => x.Subject == subject).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(GetCancellationToken(cancellationToken)))
            .OrderBy(authorization => authorization.Id!)
            .SkipIf<OpenIddictAuthorization, IQueryable<OpenIddictAuthorization>>(offset.HasValue, offset)
            .TakeIf<OpenIddictAuthorization, IQueryable<OpenIddictAuthorization>>(count.HasValue, count)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<long> PruneAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var tokenIds = await (await GetQueryableAsync<OpenIddictToken>(cancellationToken))
            .Where(x => x.AuthorizationId != null)
            .Select(x => x.AuthorizationId.Value)
            .ToListAsync(GetCancellationToken(cancellationToken));

        var authorizations = await (await GetQueryableAsync(cancellationToken))
            .Where(x => x.CreationDate < date)
            .Where(x => x.Status != OpenIddictConstants.Statuses.Valid || (x.Type == OpenIddictConstants.AuthorizationTypes.AdHoc && !tokenIds.Contains(x.Id)))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        var tokens = await (await GetQueryableAsync<OpenIddictToken>(cancellationToken))
            .Where(x => x.AuthorizationId != null && authorizations.Contains(x.AuthorizationId.Value))
            .ToListAsync(cancellationToken: cancellationToken);

        if (tokens.Any())
        {
            var tokenDbContext = await GetDbContextAsync(cancellationToken);
            if (tokenDbContext.SessionHandle != null)
            {
                await tokenDbContext.Collection<OpenIddictToken>().DeleteManyAsync(
                    tokenDbContext.SessionHandle,
                    await TokenDbRepositoryFilterer.CreateEntitiesFilterAsync(tokens),
                    cancellationToken: cancellationToken);
            }
            else
            {
                await tokenDbContext.Collection<OpenIddictToken>().DeleteManyAsync(
                    await TokenDbRepositoryFilterer.CreateEntitiesFilterAsync(tokens),
                    cancellationToken: cancellationToken);
            }
        }

        await DeleteManyAsync(authorizations, cancellationToken: cancellationToken);
        return authorizations.Count;
    }

    public async ValueTask<long> RevokeAsync(string subject, Guid? applicationId, string status, string type, CancellationToken cancellationToken = default)
    {
        var filter = Builders<OpenIddictAuthorization>.Filter.Empty;

        if (!string.IsNullOrEmpty(subject))
        {
            filter &= Builders<OpenIddictAuthorization>.Filter.Where(authorization => authorization.Subject == subject);
        }

        if (applicationId.HasValue)
        {
            filter &= Builders<OpenIddictAuthorization>.Filter.Where(authorization => authorization.ApplicationId == applicationId);
        }

        if (!string.IsNullOrEmpty(status))
        {
            filter &= Builders<OpenIddictAuthorization>.Filter.Where(authorization => authorization.Status == status);
        }

        if (!string.IsNullOrEmpty(type))
        {
            filter &= Builders<OpenIddictAuthorization>.Filter.Where(authorization => authorization.Type == type);
        }

        return (await (await GetCollectionAsync(cancellationToken)).UpdateManyAsync(
            filter           : filter,
            update           : Builders<OpenIddictAuthorization>.Update.Set(authorization => authorization.Status, OpenIddictConstants.Statuses.Revoked),
            options          : null,
            cancellationToken: cancellationToken)).MatchedCount;
    }

    public virtual async ValueTask<long> RevokeByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        return (await (await GetCollectionAsync(cancellationToken)).UpdateManyAsync(
            filter           : authorization => authorization.ApplicationId == applicationId,
            update           : Builders<OpenIddictAuthorization>.Update.Set(authorization => authorization.Status, OpenIddictConstants.Statuses.Revoked),
            options          : null,
            cancellationToken: cancellationToken)).MatchedCount;
    }

    public virtual async ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        return (await (await GetCollectionAsync(cancellationToken)).UpdateManyAsync(
            filter           : authorization => authorization.Subject == subject,
            update           : Builders<OpenIddictAuthorization>.Update.Set(authorization => authorization.Status, OpenIddictConstants.Statuses.Revoked),
            options          : null,
            cancellationToken: cancellationToken)).MatchedCount;
    }
}
