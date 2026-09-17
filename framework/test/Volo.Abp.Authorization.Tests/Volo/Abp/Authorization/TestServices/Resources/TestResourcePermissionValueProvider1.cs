using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;

namespace Volo.Abp.Authorization.TestServices.Resources;

public class TestResourcePermissionValueProvider1 : ResourcePermissionValueProvider
{
    public TestResourcePermissionValueProvider1(IResourcePermissionStore permissionStore) : base(permissionStore)
    {
    }

    public override string Name => "TestResourcePermissionValueProvider1";

    public override Task<PermissionGrantResult> CheckAsync(ResourcePermissionValueCheckContext context)
    {
        var result = PermissionGrantResult.Undefined;
        if (context.Permission.Name == "MyResourcePermission6" &&
            context.ResourceName == TestEntityResource.ResourceName &&
            context.ResourceKey == TestEntityResource.ResourceKey6)
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
            result.Result.Add(name, name == "MyResourcePermission6" &&
                                    context.ResourceName == TestEntityResource.ResourceName &&
                                    context.ResourceKey == TestEntityResource.ResourceKey6
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined);
        }

        return Task.FromResult(result);
    }
}
