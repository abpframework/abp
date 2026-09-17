using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;

namespace Volo.Abp.Authorization.TestServices.Resources;

public class TestResourcePermissionValueProvider2 : ResourcePermissionValueProvider
{
    public TestResourcePermissionValueProvider2(IResourcePermissionStore permissionStore) : base(permissionStore)
    {
    }

    public override string Name => "TestResourcePermissionValueProvider2";

    public override Task<PermissionGrantResult> CheckAsync(ResourcePermissionValueCheckContext context)
    {
        var result = PermissionGrantResult.Undefined;
        if (context.Permission.Name == "MyResourcePermission7" &&
            context.ResourceName == TestEntityResource.ResourceName &&
            context.ResourceKey == TestEntityResource.ResourceKey7)
        {
            result = PermissionGrantResult.Granted;
        }

        return Task.FromResult(result);
    }

    public override Task<MultiplePermissionGrantResult> CheckAsync(ResourcePermissionValuesCheckContext context)
    {
        var result = new MultiplePermissionGrantResult();
        foreach (var name in context.Permissions.Select(x => x.Name))
        {
            result.Result.Add(name, name == "MyResourcePermission7" &&
                                    context.ResourceName == TestEntityResource.ResourceName &&
                                    context.ResourceKey == TestEntityResource.ResourceKey7
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined);
        }

        return Task.FromResult(result);
    }
}
