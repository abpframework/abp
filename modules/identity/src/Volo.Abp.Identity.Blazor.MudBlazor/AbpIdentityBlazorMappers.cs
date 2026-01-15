using Volo.Abp.Mapperly;

namespace Volo.Abp.Identity.Blazor.MudBlazor;

[Mapper(UseDeepCloning = true)]
public static partial class AbpIdentityBlazorMappers
{
    public static partial IdentityUserDto Clone(this IdentityUserDto source);
    public static partial IdentityRoleDto Clone(this IdentityRoleDto source);
}
