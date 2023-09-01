using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions;

public interface ICanAddChildPermission
{
    PermissionDefinition AddPermission(
        [NotNull] string name,
        ILocalizableString? displayName = null,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both,
        bool isEnabled = true);
}