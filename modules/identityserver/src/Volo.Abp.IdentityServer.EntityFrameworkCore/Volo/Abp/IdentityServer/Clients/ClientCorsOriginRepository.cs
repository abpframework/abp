using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace Volo.Abp.IdentityServer.Clients
{

    public class ClientCorsOriginRepository : EfCoreRepository<IIdentityServerDbContext, ClientCorsOrigin>, IClientCorsOriginRepository
    {


        public ClientCorsOriginRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Get All Client Cors Origins
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetAllClientCorsOrigins(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await DbSet.Select(p => p.Origin).ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
