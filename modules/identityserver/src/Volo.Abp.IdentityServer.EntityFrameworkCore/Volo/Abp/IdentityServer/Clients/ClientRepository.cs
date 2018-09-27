using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

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

        public override IQueryable<Client> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
