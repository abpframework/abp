using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement.Integration;

namespace Volo.Abp.PermissionManagement;

[Dependency(TryRegister = true)]
public class HttpClientPermissionFinder : IPermissionFinder, ITransientDependency
{
    protected IPermissionIntegrationService PermissionIntegrationService { get; }

    public HttpClientPermissionFinder(IPermissionIntegrationService permissionIntegrationService)
    {
        PermissionIntegrationService = permissionIntegrationService;
    }

    public virtual async Task<List<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> requests)
    {
        return (await PermissionIntegrationService.IsGrantedAsync(requests)).Items.ToList();
    }
}
