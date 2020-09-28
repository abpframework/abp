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

        public virtual async Task<Client> FindByCliendIdAsync(
            string clientId,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(x => x.ClientId == clientId, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Client>> GetListAsync(
            string sorting, int skipCount, int maxResultCount, string filter, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.ClientId.Contains(filter))
                .OrderBy(sorting ?? nameof(Client.ClientName) + " desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<string>> GetAllDistinctAllowedCorsOriginsAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.ClientCorsOrigins
                .Select(x => x.Origin)
                .Distinct()
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> CheckClientIdExistAsync(string clientId, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(c => c.Id != expectedId && c.ClientId == clientId, cancellationToken: cancellationToken);
        }

        public override async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var clientGrantType in DbContext.Set<ClientGrantType>().Where(x => x.ClientId == id))
            {
                DbContext.Set<ClientGrantType>().Remove(clientGrantType);
            }

            foreach (var clientRedirectUri in DbContext.Set<ClientRedirectUri>().Where(x => x.ClientId == id))
            {
                DbContext.Set<ClientRedirectUri>().Remove(clientRedirectUri);
            }

            foreach (var clientPostLogoutRedirectUri in DbContext.Set<ClientPostLogoutRedirectUri>().Where(x => x.ClientId == id))
            {
                DbContext.Set<ClientPostLogoutRedirectUri>().Remove(clientPostLogoutRedirectUri);
            }

            foreach (var clientScope in DbContext.Set<ClientScope>().Where(x => x.ClientId == id))
            {
                DbContext.Set<ClientScope>().Remove(clientScope);
            }

            foreach (var clientSecret in DbContext.Set<ClientSecret>().Where(x => x.ClientId == id))
            {
                DbContext.Set<ClientSecret>().Remove(clientSecret);
            }

            foreach (var clientClaim in DbContext.Set<ClientClaim>().Where(x => x.ClientId == id))
            {
                DbContext.Set<ClientClaim>().Remove(clientClaim);
            }

            foreach (var clientIdPRestriction in DbContext.Set<ClientIdPRestriction>().Where(x => x.ClientId == id))
            {
                DbContext.Set<ClientIdPRestriction>().Remove(clientIdPRestriction);
            }

            foreach (var clientCorsOrigin in DbContext.Set<ClientCorsOrigin>().Where(x => x.ClientId == id))
            {
                DbContext.Set<ClientCorsOrigin>().Remove(clientCorsOrigin);
            }

            foreach (var clientProperty in DbContext.Set<ClientProperty>().Where(x => x.ClientId == id))
            {
                DbContext.Set<ClientProperty>().Remove(clientProperty);
            }

            await base.DeleteAsync(id, autoSave, cancellationToken);
        }

        public override IQueryable<Client> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
