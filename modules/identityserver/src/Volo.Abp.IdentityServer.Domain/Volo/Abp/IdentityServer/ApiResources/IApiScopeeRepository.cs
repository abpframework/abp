using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public interface IApiScopeRepository : IBasicRepository<ApiScope>
    {
        Task<List<ApiScope>> GetListByNameAsync(
            string[] scopeNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );
    }
}
