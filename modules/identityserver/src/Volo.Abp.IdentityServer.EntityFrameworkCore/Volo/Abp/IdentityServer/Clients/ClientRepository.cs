using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientRepository : EfCoreRepository<IIdentityServerDbContext, Client, Guid>, IClientRepository
    {
        public ClientRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public virtual async Task<Client> FindByClientIdAsync(
            string clientId,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .OrderBy(x => x.ClientId)
                .FirstOrDefaultAsync(x => x.ClientId == clientId, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Client>> GetListAsync(
            string sorting, int skipCount, int maxResultCount, string filter, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.ClientId.Contains(filter))
                .OrderBy(sorting ?? nameof(Client.ClientName) + " desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.ClientId.Contains(filter))
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<string>> GetAllDistinctAllowedCorsOriginsAsync(CancellationToken cancellationToken = default)
        {
            return await (await GetDbContextAsync()).ClientCorsOrigins
                .Select(x => x.Origin)
                .Distinct()
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> CheckClientIdExistAsync(string clientId, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).AnyAsync(c => c.Id != expectedId && c.ClientId == clientId, cancellationToken: cancellationToken);
        }

        public async override Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            foreach (var clientGrantType in dbContext.Set<ClientGrantType>().Where(x => x.ClientId == id))
            {
                dbContext.Set<ClientGrantType>().Remove(clientGrantType);
            }

            foreach (var clientRedirectUri in dbContext.Set<ClientRedirectUri>().Where(x => x.ClientId == id))
            {
                dbContext.Set<ClientRedirectUri>().Remove(clientRedirectUri);
            }

            foreach (var clientPostLogoutRedirectUri in dbContext.Set<ClientPostLogoutRedirectUri>().Where(x => x.ClientId == id))
            {
                dbContext.Set<ClientPostLogoutRedirectUri>().Remove(clientPostLogoutRedirectUri);
            }

            foreach (var clientScope in dbContext.Set<ClientScope>().Where(x => x.ClientId == id))
            {
                dbContext.Set<ClientScope>().Remove(clientScope);
            }

            foreach (var clientSecret in dbContext.Set<ClientSecret>().Where(x => x.ClientId == id))
            {
                dbContext.Set<ClientSecret>().Remove(clientSecret);
            }

            foreach (var clientClaim in dbContext.Set<ClientClaim>().Where(x => x.ClientId == id))
            {
                dbContext.Set<ClientClaim>().Remove(clientClaim);
            }

            foreach (var clientIdPRestriction in dbContext.Set<ClientIdPRestriction>().Where(x => x.ClientId == id))
            {
                dbContext.Set<ClientIdPRestriction>().Remove(clientIdPRestriction);
            }

            foreach (var clientCorsOrigin in dbContext.Set<ClientCorsOrigin>().Where(x => x.ClientId == id))
            {
                dbContext.Set<ClientCorsOrigin>().Remove(clientCorsOrigin);
            }

            foreach (var clientProperty in dbContext.Set<ClientProperty>().Where(x => x.ClientId == id))
            {
                dbContext.Set<ClientProperty>().Remove(clientProperty);
            }

            await base.DeleteAsync(id, autoSave, cancellationToken);
        }

        [Obsolete("Use WithDetailsAsync method.")]
        public override IQueryable<Client> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public override async Task<IQueryable<Client>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
