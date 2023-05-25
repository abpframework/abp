using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
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

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.Subject == subject && x.ApplicationId == client)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.Subject == subject && x.Status == status && x.ApplicationId == client)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, string type, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.Subject == subject && x.Status == status && x.Type == type && x.ApplicationId == client)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))).Where(x => x.ApplicationId == applicationId).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictAuthorization> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))).FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken))).Where(x => x.Subject == subject).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(GetCancellationToken(cancellationToken)))
            .OrderBy(authorization => authorization.Id!)
            .SkipIf<OpenIddictAuthorization, IQueryable<OpenIddictAuthorization>>(offset.HasValue, offset)
            .TakeIf<OpenIddictAuthorization, IQueryable<OpenIddictAuthorization>>(count.HasValue, count)
            .As<IMongoQueryable<OpenIddictAuthorization>>().ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task PruneAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var tokenIds = await (await GetMongoQueryableAsync<OpenIddictToken>(cancellationToken))
            .Where(x => x.AuthorizationId != null)
            .Select(x => x.AuthorizationId.Value)
            .ToListAsync(GetCancellationToken(cancellationToken));

        var authorizations = await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.CreationDate < date)
            .Where(x => x.Status != OpenIddictConstants.Statuses.Valid || (x.Type == OpenIddictConstants.AuthorizationTypes.AdHoc && !tokenIds.Contains(x.Id)))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        var tokens = await (await GetMongoQueryableAsync<OpenIddictToken>(cancellationToken))
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
    }
}
