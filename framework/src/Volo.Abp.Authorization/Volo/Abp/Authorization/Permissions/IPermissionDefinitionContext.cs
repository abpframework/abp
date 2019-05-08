using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionDefinitionContext
    {
        //TODO: Add Get methods to find and modify a permission or group.
        PermissionGroupDefinition GetGroupOrNull(string name);

        PermissionGroupDefinition AddGroup(
            [NotNull] string name, 
            ILocalizableString displayName = null,
            MultiTenancySides multiTenancySide = MultiTenancySides.Both);

        void RemoveGroup(string name);
    }
}