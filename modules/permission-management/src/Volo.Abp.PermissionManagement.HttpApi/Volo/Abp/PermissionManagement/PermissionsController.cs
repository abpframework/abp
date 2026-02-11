using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.PermissionManagement;

[RemoteService(Name = PermissionManagementRemoteServiceConsts.RemoteServiceName)]
[Area(PermissionManagementRemoteServiceConsts.ModuleName)]
[Route("api/permission-management/permissions")]
public class PermissionsController : AbpControllerBase, IPermissionAppService
{
    protected IPermissionAppService PermissionAppService { get; }

    public PermissionsController(IPermissionAppService permissionAppService)
    {
        PermissionAppService = permissionAppService;
    }

    [HttpGet]
    public virtual Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
    {
        return PermissionAppService.GetAsync(providerName, providerKey);
    }

    [HttpGet]
    [Route("by-group")]
    public virtual Task<GetPermissionListResultDto> GetByGroupAsync(string groupName, string providerName, string providerKey)
    {
        return PermissionAppService.GetByGroupAsync(groupName, providerName, providerKey);
    }

    [HttpPut]
    public virtual Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
    {
        return PermissionAppService.UpdateAsync(providerName, providerKey, input);
    }

    [HttpGet("resource-provider-key-lookup-services")]
    public virtual Task<GetResourceProviderListResultDto> GetResourceProviderKeyLookupServicesAsync(string resourceName)
    {
        return PermissionAppService.GetResourceProviderKeyLookupServicesAsync(resourceName);
    }

    [HttpGet("search-resource-provider-keys")]
    public virtual Task<SearchProviderKeyListResultDto> SearchResourceProviderKeyAsync(string resourceName, string serviceName, string filter, int page)
    {
        return PermissionAppService.SearchResourceProviderKeyAsync(resourceName, serviceName, filter, page);
    }

    [HttpGet("resource-definitions")]
    public virtual Task<GetResourcePermissionDefinitionListResultDto> GetResourceDefinitionsAsync(string resourceName)
    {
        return PermissionAppService.GetResourceDefinitionsAsync(resourceName);
    }

    [HttpGet]
    [Route("resource")]
    public virtual Task<GetResourcePermissionListResultDto> GetResourceAsync(string resourceName, string resourceKey)
    {
        return PermissionAppService.GetResourceAsync(resourceName, resourceKey);
    }

    [HttpGet]
    [Route("resource/by-provider")]
    public virtual Task<GetResourcePermissionWithProviderListResultDto> GetResourceByProviderAsync(string resourceName, string resourceKey, string providerName, string providerKey)
    {
        return PermissionAppService.GetResourceByProviderAsync(resourceName, resourceKey, providerName, providerKey);
    }

    [HttpPut]
    [Route("resource")]
    public virtual Task UpdateResourceAsync(string resourceName, string resourceKey, UpdateResourcePermissionsDto input)
    {
        return PermissionAppService.UpdateResourceAsync(resourceName, resourceKey, input);
    }

    [HttpDelete]
    [Route("resource")]
    public virtual Task DeleteResourceAsync(string resourceName, string resourceKey, string providerName, string providerKey)
    {
        return PermissionAppService.DeleteResourceAsync(resourceName, resourceKey, providerName, providerKey);
    }
}
