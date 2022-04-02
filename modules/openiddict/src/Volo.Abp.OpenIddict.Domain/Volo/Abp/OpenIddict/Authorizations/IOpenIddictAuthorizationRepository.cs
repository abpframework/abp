using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Authorizations;

public interface IOpenIddictAuthorizationRepository : IBasicRepository<OpenIddictAuthorization, Guid>
{
    Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictAuthorization>, IQueryable<TResult>> query, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> FindAsync(string subject, Guid client, string status, string type, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> FindByApplicationIdAsync(Guid applicationId, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<OpenIddictAuthorization> FindByIdAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> FindBySubjectAsync(string subject, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query, TState state, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> ListAsync(int? count, int? offset, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorization>, TState, IQueryable<TResult>> query, TState state, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictAuthorization>> GetPruneListAsync(DateTime date, int count, bool includeDetails = true, CancellationToken cancellationToken = default);
}
