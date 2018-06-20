using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionDefinitionContext
    {
        //TODO: Add Get methods to find and modify a permission or group.

        PermissionGroupDefinition AddGroup([NotNull] string name, ILocalizableString displayName = null);
    }
}