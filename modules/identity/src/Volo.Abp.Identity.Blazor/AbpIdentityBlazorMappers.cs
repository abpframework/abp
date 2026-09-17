using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Volo.Abp.Identity.Blazor;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class IdentityUserDtoToIdentityUserUpdateDtoMapper : MapperBase<IdentityUserDto, IdentityUserUpdateDto>
{
    [MapperIgnoreTarget(nameof(IdentityUserUpdateDto.Password))]
    [MapperIgnoreTarget(nameof(IdentityUserUpdateDto.RoleNames))]
    public override partial IdentityUserUpdateDto Map(IdentityUserDto source);

    [MapperIgnoreTarget(nameof(IdentityUserUpdateDto.Password))]
    [MapperIgnoreTarget(nameof(IdentityUserUpdateDto.RoleNames))]
    public override partial void Map(IdentityUserDto source, IdentityUserUpdateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class IdentityRoleDtoToIdentityRoleUpdateDtoMapper : MapperBase<IdentityRoleDto, IdentityRoleUpdateDto>
{
    public override partial IdentityRoleUpdateDto Map(IdentityRoleDto source);
    public override partial void Map(IdentityRoleDto source, IdentityRoleUpdateDto destination);
}
