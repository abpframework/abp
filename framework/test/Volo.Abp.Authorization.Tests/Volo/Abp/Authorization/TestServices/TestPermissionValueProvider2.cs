using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Authorization.TestServices;

public class TestPermissionValueProvider2 : PermissionValueProvider
{
    public TestPermissionValueProvider2(IPermissionStore permissionStore) : base(permissionStore)
    {
    }

    public override string Name => "TestPermissionValueProvider2";

    public override Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        var result = PermissionGrantResult.Undefined;
        if (context.Permission.Name == "MyPermission7")
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
            result.Result.Add(name, name == "MyPermission7"
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined);
        }

        return Task.FromResult(result);
    }
}