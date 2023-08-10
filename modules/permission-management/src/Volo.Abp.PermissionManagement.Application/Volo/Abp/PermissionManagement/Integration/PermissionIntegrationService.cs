using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement.Integration;

[IntegrationService]
public class PermissionIntegrationService : ApplicationService, IPermissionIntegrationService
{
    protected IPermissionManager PermissionManager { get; }

    public PermissionIntegrationService(IPermissionManager permissionManager)
    {
        PermissionManager = permissionManager;
    }

    public virtual async Task<IsGrantedOutput> IsGrantedAsync(IsGrantedInput input)
    {
        return new IsGrantedOutput
        {
            UserId = input.UserId,
            GrantedPermissionNames = (await PermissionManager.GetAsync(input.PermissionNames, UserPermissionValueProvider.ProviderName, input.UserId.ToString())).Result.Where(x => x.IsGranted).Select(x => x.Name).ToList()
        };
    }

    public virtual async Task<List<IsGrantedOutput>> IsGrantedAsync(List<IsGrantedInput> input)
    {
        var result = new List<IsGrantedOutput>();
        foreach (var item in input)
        {
            result.Add(new IsGrantedOutput
            {
                UserId = item.UserId,
                GrantedPermissionNames = (await PermissionManager.GetAsync(item.PermissionNames, UserPermissionValueProvider.ProviderName, item.UserId.ToString())).Result.Where(x => x.IsGranted).Select(x => x.Name).ToList()
            });
        }

        return result;
    }
}
