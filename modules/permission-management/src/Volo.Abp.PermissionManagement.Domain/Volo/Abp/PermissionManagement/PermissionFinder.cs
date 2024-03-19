using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.PermissionManagement;

public class PermissionFinder : IPermissionFinder, ITransientDependency
{
    protected IPermissionManager PermissionManager { get; }

    public PermissionFinder(IPermissionManager permissionManager)
    {
        PermissionManager = permissionManager;
    }

    public virtual async Task<List<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> requests)
    {
        var result = new List<IsGrantedResponse>();
        foreach (var item in requests)
        {
            result.Add(new IsGrantedResponse
            {
                UserId = item.UserId,
                Permissions = (await PermissionManager.GetAsync(item.PermissionNames, UserPermissionValueProvider.ProviderName, item.UserId.ToString())).Result
                    .ToDictionary(x => x.Name, x => x.IsGranted)
            });
        }

        return result;
    }
}
