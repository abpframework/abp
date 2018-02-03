using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace Volo.Abp.IdentityServer
{
    public class ApiResourceRepository : EfCoreRepository<IIdentityServerDbContext, ApiResource, Guid>, IApiResourceRepository
    {
        public ApiResourceRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public virtual async Task<ApiResource> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await this.FirstOrDefaultAsync(ar => ar.Name == name, cancellationToken);
        }
    }
}