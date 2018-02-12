using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Permissions
{
    public interface IPermissionStore
    {
        Task<bool> IsGrantedAsync([NotNull] string name, [CanBeNull] string providerName, [CanBeNull] string providerKey);
    }
}
