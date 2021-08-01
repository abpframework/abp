using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.OpenIddict.Scopes
{
    public interface IOpenIddictScopeRepository : IRepository<OpenIddictScope, Guid>
    {
        Task<OpenIddictScope> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictScope>> FindByNamesAsync(
            List<string> names,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictScope>> FindByResourceAsync(
            string resource,
            CancellationToken cancellationToken = default);

        Task<List<OpenIddictScope>> GetListAsync(
            int? maxResultCount,
            int? skipCount,
            CancellationToken cancellationToken = default);
    }
}