using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using System.Linq.Dynamic.Core;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.MongoDB;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class MongoClientRepository : MongoDbRepository<IAbpIdentityServerMongoDbContext, Client, Guid>, IClientRepository
    {
        public MongoClientRepository(IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<Client> FindByCliendIdAsync(string clientId, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().FirstOrDefaultAsync(x => x.ClientId == clientId, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Client>> GetListAsync(string sorting, int skipCount, int maxResultCount, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .OrderBy(sorting ?? nameof(Client.ClientName))
                .As<IMongoQueryable<Client>>()
                .PageBy<Client, IMongoQueryable<Client>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetTotalCount()
        {
            return await GetCountAsync();
        }
    }
}
