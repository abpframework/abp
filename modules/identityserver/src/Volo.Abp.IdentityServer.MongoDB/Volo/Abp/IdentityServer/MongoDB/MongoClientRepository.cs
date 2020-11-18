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
        public MongoClientRepository(
            IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider
            ) : base(
                dbContextProvider)
        {
        }

        public virtual async Task<Client> FindByClientIdAsync(
            string clientId,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(x => x.ClientId == clientId)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Client>> GetListAsync(
            string sorting,
            int skipCount,
            int maxResultCount,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .WhereIf(!filter.IsNullOrWhiteSpace(), x=>x.ClientId.Contains(filter))
                .OrderBy(sorting ?? nameof(Client.ClientName))
                .As<IMongoQueryable<Client>>()
                .PageBy<Client, IMongoQueryable<Client>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<string>> GetAllDistinctAllowedCorsOriginsAsync(
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .SelectMany(x => x.AllowedCorsOrigins)
                .Select(y => y.Origin)
                .Distinct()
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> CheckClientIdExistAsync(string clientId, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().AnyAsync(c => c.Id != expectedId && c.ClientId == clientId, cancellationToken: cancellationToken);
        }

        public virtual async Task<long> GetTotalCount()
        {
            return await GetCountAsync();
        }
    }
}
