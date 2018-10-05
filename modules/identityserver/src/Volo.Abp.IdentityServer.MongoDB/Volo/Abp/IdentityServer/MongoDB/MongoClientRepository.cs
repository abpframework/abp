using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class MongoClientRepository : MongoDbRepository<IAbpIdentityServerMongoDbContext, Client, Guid>, IClientRepository
    {
        public MongoClientRepository(IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Client> FindByCliendIdAsync(string clientId, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().FirstOrDefaultAsync(x => x.ClientId == clientId, GetCancellationToken(cancellationToken));
        }
    }
}
