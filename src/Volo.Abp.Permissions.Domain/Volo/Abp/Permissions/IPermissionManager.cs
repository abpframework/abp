using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Permissions
{
    public interface IPermissionManager
    {
        Task<PermissionWithGrantedProviders> GetAsync(string name, string providerName, string providerKey);

        Task<List<PermissionWithGrantedProviders>> GetAllAsync([NotNull] string providerName, [NotNull] string providerKey);

        //Task GrantAsync([NotNull] string name, [NotNull] string providerName, [NotNull] string providerKey);

        //Task RevokeAsync([NotNull] string name, [NotNull] string providerName, [NotNull] string providerKey);
    }
}