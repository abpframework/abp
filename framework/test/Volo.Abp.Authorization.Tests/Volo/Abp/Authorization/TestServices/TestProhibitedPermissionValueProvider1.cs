using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Authorization.TestServices;

public class TestProhibitedPermissionValueProvider1 : PermissionValueProvider
{
    public TestProhibitedPermissionValueProvider1(IPermissionStore permissionStore) : base(permissionStore)
    {
    }

    public override string Name => "TestProhibitedPermissionValueProvider1";

    public override Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        var result = PermissionGrantResult.Undefined;
        if (context.Permission.Name == "MyPermission8" || context.Permission.Name == "MyPermission9")
        {
            result = PermissionGrantResult.Granted;
        }

        return Task.FromResult(result);
    }

    public override Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
    {
        var result = new MultiplePermissionGrantResult();
        foreach (var name in context.Permissions.Select(x => x.Name))
        {
            result.Result.Add(name, name == "MyPermission8" || name == "MyPermission9"
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined);
        }

        return Task.FromResult(result);
    }
}
