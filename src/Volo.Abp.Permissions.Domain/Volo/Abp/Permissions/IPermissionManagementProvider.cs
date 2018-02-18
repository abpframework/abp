using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
{
    public interface IPermissionManagementProvider : ISingletonDependency
    {
        string Name { get; }

        Task<PermissionValueProviderGrantInfo> CheckAsync([NotNull] string name, [NotNull] string providerName, [NotNull] string providerKey);

        Task GrantAsync([NotNull] string name, [NotNull] string providerKey);

        Task RevokeAsync([NotNull] string name, [NotNull] string providerKey);
    }
}