using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Tokens;

public interface IOpenIddictTokenRepository : IBasicRepository<OpenIddictToken, Guid>
{
    Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictToken>, IQueryable<TResult>> query, CancellationToken cancellationToken);

    Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, CancellationToken cancellationToken);

    Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, CancellationToken cancellationToken);

    Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, string type, CancellationToken cancellationToken);

    Task<List<OpenIddictToken>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken);

    Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(Guid authorizationId, CancellationToken cancellationToken);

    Task<OpenIddictToken> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<OpenIddictToken> FindByReferenceIdAsync(string referenceId, CancellationToken cancellationToken);

    Task<List<OpenIddictToken>> FindBySubjectAsync(string subject, CancellationToken cancellationToken);

    Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken);

    Task<List<OpenIddictToken>> ListAsync(int? count, int? offset, CancellationToken cancellationToken);

    Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken);

    Task<List<OpenIddictToken>> GetPruneListAsync(DateTime date, int count, CancellationToken cancellationToken);
}
