using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.IdentityServer.IdentityResources;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using IdentityResource = Volo.Abp.IdentityServer.IdentityResources.IdentityResource;

namespace Volo.Abp.IdentityServer
{
    public class IdentityResourceRepository : EfCoreRepository<IdentityServerDbContext, IdentityResource>, IIdentityResourceRepository
    {
        public IdentityResourceRepository(IDbContextProvider<IdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public Task<List<IdentityResource>> FindIdentityResourcesByScopeAsync(string[] scopeNames)
        {
            var query = from identityResource in DbSet.Include(x => x.UserClaims)
                        where scopeNames.Contains(identityResource.Name)
                        select identityResource;

            return query.ToListAsync();
        }

        public Task<List<ApiResource>> FindApiResourcesByScopeAsync(string[] scopeNames)
        {
            var names = scopeNames.ToArray();

            var query = from api in DbContext.ApiResources
                        where api.Scopes.Any(x => names.Contains(x.Name))
                        select api;

            var apis = query
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims);

            return apis.ToListAsync();
        }

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            var query = from apiResource in DbContext.ApiResources
                        where apiResource.Name == name
                        select apiResource;

            var apis = query
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims);

            return apis.FirstOrDefaultAsync();
        }

        public Task<ApiResources.ApiResources> GetAllResourcesAsync()
        {
            var identity = DbContext.IdentityResources
                .Include(x => x.UserClaims);

            var apis = DbContext.ApiResources
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims);

            return Task.FromResult(new ApiResources.ApiResources(identity.ToArrayAsync(), apis.ToArrayAsync()));
        }
    }
}
