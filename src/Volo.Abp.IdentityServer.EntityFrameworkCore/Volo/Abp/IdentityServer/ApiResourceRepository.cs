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
    public class ApiResourceRepository : EfCoreRepository<IdentityServerDbContext, ApiResource, Guid>, IApiResourceRepository
    {
        public ApiResourceRepository(IDbContextProvider<IdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<ApiResource> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await this.FirstOrDefaultAsync(ar => ar.Name == name, cancellationToken);
        }
    }
}