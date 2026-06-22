using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions.Resources;

public abstract class ResourcePermissionValueProvider : IResourcePermissionValueProvider, ITransientDependency
{
    public abstract string Name { get; }

    protected IResourcePermissionStore ResourcePermissionStore { get; }

    protected ResourcePermissionValueProvider(IResourcePermissionStore resourcePermissionStore)
    {
        ResourcePermissionStore = resourcePermissionStore;
    }

    public abstract Task<PermissionGrantResult> CheckAsync(ResourcePermissionValueCheckContext context);

    public abstract Task<MultiplePermissionGrantResult> CheckAsync(ResourcePermissionValuesCheckContext context);
}
