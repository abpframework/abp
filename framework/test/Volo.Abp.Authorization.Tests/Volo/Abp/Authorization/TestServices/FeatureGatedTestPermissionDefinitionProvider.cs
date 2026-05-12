using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;

namespace Volo.Abp.Authorization.TestServices;

public class FeatureGatedTestPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var group = context.AddGroup("FeatureGatedTestGroup");

        group.AddPermission("FeatureGatedPermission")
            .RequireFeatures("FeatureGatedTestFeature");
    }
}
