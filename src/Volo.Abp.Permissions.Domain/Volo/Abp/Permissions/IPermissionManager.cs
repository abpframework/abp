using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Permissions
{
    public interface IPermissionManager
    {
        Task<bool> IsGrantedAsync([NotNull]string name, [NotNull] string providerName, [NotNull] string providerKey);

        Task<List<string>> GetAllGrantedAsync([NotNull] string providerName, [NotNull] string providerKey);

        Task GrantAsync([NotNull] string name, [NotNull] string providerName, [NotNull] string providerKey);

        Task RevokeAsync([NotNull] string name, [NotNull] string providerName, [NotNull] string providerKey);
    }
}