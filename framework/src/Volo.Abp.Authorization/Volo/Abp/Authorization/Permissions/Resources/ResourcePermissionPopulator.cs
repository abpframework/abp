using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions.Resources;

public class ResourcePermissionPopulator : ITransientDependency
{
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
    protected IResourcePermissionChecker ResourcePermissionChecker { get; }
    protected IResourcePermissionStore ResourcePermissionStore { get; }
    protected IPermissionChecker PermissionChecker { get; }

    public ResourcePermissionPopulator(
        IPermissionDefinitionManager permissionDefinitionManager,
        IResourcePermissionChecker resourcePermissionChecker,
        IResourcePermissionStore resourcePermissionStore,
        IPermissionChecker permissionChecker)
    {
        PermissionDefinitionManager = permissionDefinitionManager;
        ResourcePermissionChecker = resourcePermissionChecker;
        ResourcePermissionStore = resourcePermissionStore;
        PermissionChecker = permissionChecker;
    }

    public virtual async Task PopulateAsync<TResource>(TResource resource, string resourceName)
        where TResource : IHasResourcePermissions
    {
        await PopulateAsync([resource], resourceName);
    }

    public virtual async Task PopulateAsync<TResource>(List<TResource> resources, string resourceName)
        where TResource : IHasResourcePermissions
    {
        Check.NotNull(resources, nameof(resources));
        Check.NotNullOrWhiteSpace(resourceName, nameof(resourceName));

        var resopurcePermissions = (await PermissionDefinitionManager.GetResourcePermissionsAsync())
            .Where(x => x.ResourceName == resourceName)
            .ToArray();

        foreach (var resource in resources)
        {
            var resourceKey = resource.GetObjectKey();
            if (resourceKey.IsNullOrEmpty())
            {
                throw new AbpException("Resource key can not be null or empty.");
            }

            var results = await ResourcePermissionChecker.IsGrantedAsync(resopurcePermissions.Select(x => x.Name).ToArray(), resourceName, resourceKey);
            foreach (var resopurcePermission in resopurcePermissions)
            {
                if (resource.ResourcePermissions == null)
                {
                     ObjectHelper.TrySetProperty(resource, x => x.ResourcePermissions, () => new Dictionary<string, bool>());
                }

                var hasPermission = results.Result.TryGetValue(resopurcePermission.Name, out var granted) && granted == PermissionGrantResult.Granted;
                resource.ResourcePermissions![resopurcePermission.Name] = hasPermission;
            }
        }
    }
}
