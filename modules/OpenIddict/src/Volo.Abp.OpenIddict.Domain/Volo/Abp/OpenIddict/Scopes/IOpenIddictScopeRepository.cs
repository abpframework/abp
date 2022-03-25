using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Scopes;

public interface IOpenIddictScopeRepository : IBasicRepository<OpenIddictScope, Guid>
{
    Task<long> CountAsync<TResult>(Func<IQueryable<OpenIddictScope>, IQueryable<TResult>> query, CancellationToken cancellationToken);

    Task<OpenIddictScope> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<OpenIddictScope> FindByNameAsync(string name, CancellationToken cancellationToken);

    Task<List<OpenIddictScope>> FindByNamesAsync(string[] names, CancellationToken cancellationToken);

    Task<List<OpenIddictScope>> FindByResourceAsync(string resource, CancellationToken cancellationToken);

    Task<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictScope>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken);

    Task<List<OpenIddictScope>> ListAsync(int? count, int? offset, CancellationToken cancellationToken);

    Task<List<TResult>> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictScope>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken);

}
