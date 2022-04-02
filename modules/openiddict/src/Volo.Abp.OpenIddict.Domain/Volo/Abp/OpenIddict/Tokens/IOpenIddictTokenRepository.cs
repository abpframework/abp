using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Tokens;

public interface IOpenIddictTokenRepository : IBasicRepository<OpenIddictToken, Guid>
{
    Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictToken>, IQueryable<TResult>> query, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, string type, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindByApplicationIdAsync(Guid applicationId, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(Guid authorizationId, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<OpenIddictToken> FindByIdAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<OpenIddictToken> FindByReferenceIdAsync(string referenceId, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindBySubjectAsync(string subject, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> ListAsync(int? count, int? offset, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> GetPruneListAsync(DateTime date, int count, bool includeDetails = true, CancellationToken cancellationToken = default);
}
