using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Scopes;

public interface IOpenIddictScopeRepository : IBasicRepository<OpenIddictScope, Guid>
{
    Task<List<OpenIddictScope>> GetListAsync(string sorting, int skipCount, int maxResultCount, string filter = null, CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default);
    
    Task<OpenIddictScope> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<OpenIddictScope> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<List<OpenIddictScope>> FindByNamesAsync(string[] names, CancellationToken cancellationToken = default);

    Task<List<OpenIddictScope>> FindByResourceAsync(string resource, CancellationToken cancellationToken = default);

    Task<List<OpenIddictScope>> ListAsync(int? count, int? offset, CancellationToken cancellationToken = default);
}
