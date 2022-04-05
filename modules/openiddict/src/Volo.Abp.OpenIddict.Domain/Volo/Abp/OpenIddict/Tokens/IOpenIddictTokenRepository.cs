using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Tokens;

public interface IOpenIddictTokenRepository : IBasicRepository<OpenIddictToken, Guid>
{
    Task DeleteManyByApplicationIdAsync(Guid applicationId, bool autoSave = false, CancellationToken cancellationToken = default);

    Task DeleteManyByAuthorizationIdAsync(Guid authorizationId, bool autoSave = false, CancellationToken cancellationToken = default);

    Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictToken>, IQueryable<TResult>> query, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindAsync(string subject, Guid client, string status, string type, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindByApplicationIdAsync(Guid applicationId, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(Guid authorizationId, CancellationToken cancellationToken = default);

    Task<OpenIddictToken> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<OpenIddictToken> FindByReferenceIdAsync(string referenceId, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> FindBySubjectAsync(string subject, CancellationToken cancellationToken = default);

    Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default);

    Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictToken>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken = default);

    Task<List<OpenIddictToken>> GetPruneListAsync(DateTime date, int count, CancellationToken cancellationToken = default);
}
