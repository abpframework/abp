using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Volo.Abp.Domain.Repositories;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public interface IIdentityResourceRepository : IRepository<IdentityResource>
    {
        Task<List<IdentityResource>> FindIdentityResourcesByScopeAsync(string[] scopeNames);

        Task<List<ApiResource>> FindApiResourcesByScopeAsync(string[] scopeNames);

        Task<ApiResource> FindApiResourceAsync(string name);

        Task<ApiResources.ApiResources> GetAllResourcesAsync();
    }
}