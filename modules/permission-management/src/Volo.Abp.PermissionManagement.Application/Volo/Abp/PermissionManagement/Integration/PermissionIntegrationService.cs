using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.PermissionManagement.Integration;

[IntegrationService]
public class PermissionIntegrationService : ApplicationService, IPermissionIntegrationService
{
    protected IPermissionFinder PermissionFinder { get; }

    public PermissionIntegrationService(IPermissionFinder permissionFinder)
    {
        PermissionFinder = permissionFinder;
    }

    public virtual async Task<ListResultDto<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> input)
    {
        return new ListResultDto<IsGrantedResponse>(await PermissionFinder.IsGrantedAsync(input));
    }
}
