using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.ApiScopes
{
    public interface IApiScopeRepository : IBasicRepository<ApiScope, Guid>
    {
        Task<ApiScope> FindByNameAsync(
            string scopeName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        Task<List<ApiScope>> GetListByNameAsync(
            string[] scopeNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<List<ApiScope>> GetListAsync(
            string sorting,
            int skipCount,
            int maxResultCount,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default
        );

        Task<bool> CheckNameExistAsync(
            string name,
            Guid? expectedId = null,
            CancellationToken cancellationToken = default
        );
    }
}
