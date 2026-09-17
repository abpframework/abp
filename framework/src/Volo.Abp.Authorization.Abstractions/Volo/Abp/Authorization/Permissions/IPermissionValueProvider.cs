using System.Threading.Tasks;

namespace Volo.Abp.Authorization.Permissions;

public interface IPermissionValueProvider
{
    string Name { get; }

    Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context);

    Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context);
}
