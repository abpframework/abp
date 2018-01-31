using System;
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
    //TODO: This is not true implementation! This repository works for 2 different aggregate root!

    public class IdentityResourceRepository : EfCoreRepository<IdentityServerDbContext, IdentityResource, Guid>, IIdentityResourceRepository
    {
        public IdentityResourceRepository(IDbContextProvider<IdentityServerDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {

        }

        public async Task<List<IdentityResource>> FindIdentityResourcesByScopeAsync(string[] scopeNames)
        {
            var query = from identityResource in DbSet.Include(x => x.UserClaims)
                        where scopeNames.Contains(identityResource.Name)
                        select identityResource;

            return await query.ToListAsync();
        }

        public async Task<List<ApiResource>> FindApiResourcesByScopeAsync(string[] scopeNames)
        {
            var query = from api in DbContext.ApiResources
                        where api.Scopes.Any(x => scopeNames.Contains(x.Name))
                        select api;

            query = query
                .Include(x => x.Secrets)
                .Include(x => x.UserClaims)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims);

            return await query.ToListAsync();
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var query = from apiResource in DbContext.ApiResources
                        where apiResource.Name == name
                        select apiResource;

            query = query
                .Include(x => x.Secrets)
                .Include(x => x.UserClaims)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ApiAndIdentityResources> GetAllResourcesAsync()
        {
            var identity = DbContext.IdentityResources
                .Include(x => x.UserClaims);

            var apis = DbContext.ApiResources
                .Include(x => x.Secrets)
                .Include(x => x.UserClaims)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims);

            return new ApiAndIdentityResources(
                await identity.ToArrayAsync(),
                await apis.ToArrayAsync()
            );
        }
    }
}
