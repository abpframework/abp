using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Application.Services;

namespace Volo.Abp.PermissionManagement;

public interface IPermissionAppService : IApplicationService
{
    Task<GetPermissionListResultDto> GetAsync([NotNull] string providerName, [NotNull] string providerKey);

    Task<GetPermissionListResultDto> GetByGroupAsync([NotNull] string groupName, [NotNull] string providerName, [NotNull] string providerKey);

    Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdatePermissionsDto input);

    Task<GetResourceProviderListResultDto> GetResourceProviderKeyLookupServicesAsync(string resourceName);

    Task<SearchProviderKeyListResultDto> SearchResourceProviderKeyAsync(string resourceName, string serviceName, string filter, int page);

    Task<GetResourcePermissionDefinitionListResultDto> GetResourceDefinitionsAsync([NotNull] string resourceName);

    Task<GetResourcePermissionListResultDto> GetResourceAsync([NotNull] string resourceName, [NotNull] string resourceKey);

    Task<GetResourcePermissionWithProviderListResultDto> GetResourceByProviderAsync([NotNull] string resourceName, [NotNull] string resourceKey, [NotNull] string providerName, [NotNull] string providerKey);

    Task UpdateResourceAsync([NotNull] string resourceName, [NotNull] string resourceKey, UpdateResourcePermissionsDto input);

    Task DeleteResourceAsync([NotNull] string resourceName, [NotNull] string resourceKey, [NotNull] string providerName, [NotNull] string providerKey);
}
