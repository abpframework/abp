using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.IdentityServer.Clients;

namespace Volo.Abp.IdentityServer
{
    public interface IClientCorsOriginRepository:IBasicRepository<ClientCorsOrigin>
    {
        /// <summary>
        /// Get All Client Cors Origins
        /// </summary> 
        /// <returns></returns>
        Task<List<string>> GetAllClientCorsOrigins(CancellationToken cancellationToken = default(CancellationToken));
    }
}
