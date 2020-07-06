using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.ApiScopes
{
    public interface IApiScopeRepository : IBasicRepository<ApiScope>
    {
        Task<ApiScope> GetByNameAsync(
            string scopeName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        Task<List<ApiScope>> GetListByNameAsync(
            string[] scopeNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );
    }
}
