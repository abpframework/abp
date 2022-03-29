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

    public virtual async Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictToken>, IQueryable<TResult>> query, CancellationToken cancellationToken)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, CancellationToken cancellationToken)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(cancellationToken)), x => x.Subject == subject && x.ApplicationId == client)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, CancellationToken cancellationToken)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(cancellationToken)), x => x.Subject == subject && x.ApplicationId == client && x.Status == status)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, string type, CancellationToken cancellationToken)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(cancellationToken)), x => x.Subject == subject && x.ApplicationId == client && x.Status == status && x.Type == type)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(cancellationToken)), x => x.ApplicationId == applicationId)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(Guid authorizationId, CancellationToken cancellationToken)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(cancellationToken)), x => x.AuthorizationId == authorizationId)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictToken> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).FirstOrDefaultAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<OpenIddictToken> FindByReferenceIdAsync(string referenceId, CancellationToken cancellationToken)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).FirstOrDefaultAsync(x => x.ReferenceId == referenceId, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        return await Queryable.Where((await GetMongoQueryableAsync(cancellationToken)), x => x.Subject == subject)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        return await query(await GetMongoQueryableAsync(cancellationToken), state).As<IMongoQueryable<TResult>>().FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<OpenIddictToken>> ListAsync(int? count, int? offset, CancellationToken cancellationToken)
    {
        return await Queryable.OrderBy((await GetMongoQueryableAsync(cancellationToken)), x => x.Id)
            .SkipIf<OpenIddictToken, IQueryable<OpenIddictToken>>(offset.HasValue, offset.Value)
            .TakeIf<OpenIddictToken, IQueryable<OpenIddictToken>>(count.HasValue, count.Value)
            .As<IMongoQueryable<OpenIddictToken>>()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        return await query(await GetMongoQueryableAsync(cancellationToken), state).As<IMongoQueryable<TResult>>().ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<List<OpenIddictToken>> GetPruneListAsync(DateTime date, int count, CancellationToken cancellationToken)
    {
        //TODO: Test & Improve?
        return await (from token in await GetMongoQueryableAsync(cancellationToken)
            join authorization in (await GetMongoQueryableAsync<OpenIddictAuthorization>(cancellationToken))
                on token.AuthorizationId equals authorization.Id into ta
            from a in ta
            where token.CreationDate < date
            where (token.Status != OpenIddictConstants.Statuses.Inactive &&
                   token.Status != OpenIddictConstants.Statuses.Valid) ||
                  (a != null && a.Status != OpenIddictConstants.Statuses.Valid) ||
                  token.ExpirationDate < DateTime.UtcNow
            orderby token.Id
            select token).Take(count).ToListAsync(GetCancellationToken(cancellationToken));
    }
}