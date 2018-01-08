using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace Volo.Abp.IdentityServer
{
    public class ApiResourceRepository : EfCoreRepository<IdentityServerDbContext, ApiResource>, IApiResourceRepository
    {
        public ApiResourceRepository(IDbContextProvider<IdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}