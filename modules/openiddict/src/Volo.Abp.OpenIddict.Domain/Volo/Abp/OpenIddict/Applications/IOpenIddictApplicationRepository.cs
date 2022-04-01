using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Applications;

public interface IOpenIddictApplicationRepository : IBasicRepository<OpenIddictApplication, Guid>
{
    Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictApplication>, IQueryable<TResult>> query, CancellationToken cancellationToken);

    Task<OpenIddictApplication> FindByClientIdAsync(string clientId, CancellationToken cancellationToken);

    Task<List<OpenIddictApplication>> FindByPostLogoutRedirectUriAsync(string address, CancellationToken cancellationToken);

    Task<List<OpenIddictApplication>> FindByRedirectUriAsync(string address, CancellationToken cancellationToken);

    Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken);

    Task<List<OpenIddictApplication>> ListAsync(int? count, int? offset, CancellationToken cancellationToken);

    Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictApplication>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken);
}
