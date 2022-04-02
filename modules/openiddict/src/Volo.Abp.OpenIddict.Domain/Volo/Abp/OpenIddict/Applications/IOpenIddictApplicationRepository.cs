using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Applications;

public interface IOpenIddictApplicationRepository : IBasicRepository<OpenIddictApplication, Guid>
{
    Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictApplication>, IQueryable<TResult>> query, CancellationToken cancellationToken = default);

    Task<OpenIddictApplication> FindByClientIdAsync(string clientId, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictApplication>> FindByPostLogoutRedirectUriAsync(string address, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictApplication>> FindByRedirectUriAsync(string address, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query, TState state, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<OpenIddictApplication>> ListAsync(int? count, int? offset, bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query, TState state, bool includeDetails = true, CancellationToken cancellationToken = default);
}
