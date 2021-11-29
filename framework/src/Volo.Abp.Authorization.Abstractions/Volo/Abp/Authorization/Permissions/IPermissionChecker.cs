using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionChecker
    {
        Task<bool> IsGrantedAsync([NotNull]string name);

        Task<bool> IsGrantedAsync([CanBeNull] ClaimsPrincipal claimsPrincipal, [NotNull]string name);

        Task<MultiplePermissionGrantResult> IsGrantedAsync([NotNull]string[] names);

        Task<MultiplePermissionGrantResult> IsGrantedAsync([CanBeNull] ClaimsPrincipal claimsPrincipal, [NotNull]string[] names);
    }
}
