using System.Security.Claims;
using System.Threading.Tasks;

namespace Volo.Abp.Authorization.Permissions.Resources;

public interface IResourcePermissionChecker
{
    Task<bool> IsGrantedAsync(
        string name,
        string resourceName,
        string resourceKey
    );

    Task<bool> IsGrantedAsync(
        ClaimsPrincipal? claimsPrincipal,
        string name,
        string resourceName,
        string resourceKey
    );

    Task<MultiplePermissionGrantResult> IsGrantedAsync(
        string[] names, 
        string resourceName,
        string resourceKey
    );

    Task<MultiplePermissionGrantResult> IsGrantedAsync(
        ClaimsPrincipal? claimsPrincipal,
        string[] names,
        string resourceName,
        string resourceKey
    );
}
