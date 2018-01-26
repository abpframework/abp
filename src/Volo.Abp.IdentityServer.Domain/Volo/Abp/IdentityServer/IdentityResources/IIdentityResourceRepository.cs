using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public interface IIdentityResourceRepository : IBasicRepository<IdentityResource, Guid>
    {
        Task<List<IdentityResource>> FindIdentityResourcesByScopeAsync(string[] scopeNames);

        Task<List<ApiResource>> FindApiResourcesByScopeAsync(string[] scopeNames);

        Task<ApiResource> FindApiResourceAsync(string name);

        Task<ApiResources.ApiAndIdentityResources> GetAllResourcesAsync();
    }
}