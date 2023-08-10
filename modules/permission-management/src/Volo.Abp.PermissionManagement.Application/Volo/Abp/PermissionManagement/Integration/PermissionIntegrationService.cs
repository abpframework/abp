using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
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

    public virtual async Task<ListResultDto<PermissionGrantOutput>> IsGrantedAsync(List<PermissionGrantInput> input)
    {
        var result = new List<PermissionGrantOutput>();
        foreach (var item in input)
        {
            result.Add(new PermissionGrantOutput
            {
                Permissions = (await PermissionManager.GetAsync(item.PermissionNames, UserPermissionValueProvider.ProviderName, item.UserId.ToString())).Result
                    .ToDictionary(x => x.Name, x => x.IsGranted)
            });
        }

        return new ListResultDto<PermissionGrantOutput>(result);
    }
}
