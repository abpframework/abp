using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace Volo.Abp.IdentityServer.ApiScopes
{
    public class ApiScopeRepository : EfCoreRepository<IIdentityServerDbContext, ApiScope>, IApiScopeRepository
    {
        public ApiScopeRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }

        public async Task<List<ApiScope>> GetListByNameAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from scope in DbSet.IncludeDetails(includeDetails)
                where scopeNames.Contains(scope.Name)
                select scope;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
