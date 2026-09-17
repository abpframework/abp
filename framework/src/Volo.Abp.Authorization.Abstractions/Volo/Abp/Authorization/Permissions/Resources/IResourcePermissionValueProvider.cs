using System.Threading.Tasks;

namespace Volo.Abp.Authorization.Permissions.Resources;

public interface IResourcePermissionValueProvider
{
    string Name { get; }

    Task<PermissionGrantResult> CheckAsync(ResourcePermissionValueCheckContext context);

    Task<MultiplePermissionGrantResult> CheckAsync(ResourcePermissionValuesCheckContext context);
}
