using Riok.Mapperly.Abstractions;
using Volo.Abp.Identity;
using Volo.Abp.Mapperly;

namespace Volo.Abp.Account;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class IdentityUserToProfileDtoMapper : MapperBase<IdentityUser, ProfileDto>
{
    [MapperIgnoreTarget(nameof(ProfileDto.HasPassword))]
    public override partial ProfileDto Map(IdentityUser source);

    [MapperIgnoreTarget(nameof(ProfileDto.HasPassword))]
    public override partial void Map(IdentityUser source, ProfileDto destination);

    public override void AfterMap(IdentityUser source, ProfileDto destination)
    {
        destination.HasPassword = source.PasswordHash != null;
    }
}