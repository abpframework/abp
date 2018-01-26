using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace Volo.Abp.IdentityServer
{
    public class ClientRepository : EfCoreRepository<IdentityServerDbContext, Client, Guid>, IClientRepository
    {
        public ClientRepository(IDbContextProvider<IdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public Task<Client> FindByCliendIdIncludingAllAsync(string clientId)
        {
            return DbSet
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.Properties)
                .FirstOrDefaultAsync(x => x.ClientId == clientId);
        }
    }
}
