using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.IdentityServer.Clients;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class MongoClientCorsOriginRepository : MongoDbRepository<IAbpIdentityServerMongoDbContext, ClientCorsOrigin>, IClientCorsOriginRepository
    {
        public MongoApiResourceRepository(IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }


        /// <summary>
        /// Get All Client Cors Origins
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetAllClientCorsOrigins(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetMongoQueryable().Select(p => p.Origin).ToListAsync(GetCancellationToken(cancellationToken));
        }

    }
}
