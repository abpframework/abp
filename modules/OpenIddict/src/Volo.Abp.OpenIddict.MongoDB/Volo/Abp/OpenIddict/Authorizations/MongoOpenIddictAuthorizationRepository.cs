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
using Volo.Abp.OpenIddict.MongoDB;

namespace Volo.Abp.OpenIddict.Authorizations;

public class MongoOpenIddictAuthorizationRepository : MongoDbRepository<OpenIddictMongoDbContext, OpenIddictAuthorization, Guid>, IOpenIddictAuthorizationRepository
{
    public MongoOpenIddictAuthorizationRepository(IMongoDbContextProvider<OpenIddictMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictAuthorization>, IQueryable<TResult>> query, CancellationToken cancellationToken)
    {
        return await query(await GetMongoQueryableAsync(cancellationToken)).As<IMongoQueryable<OpenIddictAuthorization>>().LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, CancellationToken cancellationToken)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.Subject == subject && x.ApplicationId == client)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, CancellationToken cancellationToken)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.Subject == subject && x.Status == status && x.ApplicationId == client)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, string type, CancellationToken cancellationToken)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.Subject == subject && x.Status == status && x.Type == type && x.ApplicationId == client)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).Where(x => x.ApplicationId == applicationId).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictAuthorization> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).Where(x => x.Subject == subject).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        return await query(await GetMongoQueryableAsync(cancellationToken), state).As<IMongoQueryable<TResult>>().FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictAuthorization>> ListAsync(int? count, int? offset, CancellationToken cancellationToken)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .OrderBy(authorization => authorization.Id!)
            .SkipIf<OpenIddictAuthorization, IQueryable<OpenIddictAuthorization>>(offset.HasValue, offset.Value)
            .TakeIf<OpenIddictAuthorization, IQueryable<OpenIddictAuthorization>>(count.HasValue, count.Value)
            .As<IMongoQueryable<OpenIddictAuthorization>>().ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        return await query(await GetMongoQueryableAsync(cancellationToken), state).As<IMongoQueryable<TResult>>().ToListAsync(cancellationToken);
    }

    public virtual async Task<List<OpenIddictAuthorization>> GetPruneListAsync(DateTime date, int count, CancellationToken cancellationToken)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.CreationDate < date)
            .Where(x => x.Status != OpenIddictConstants.Statuses.Valid || (x.Type == OpenIddictConstants.AuthorizationTypes.AdHoc && !x.Tokens.Any()))
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}