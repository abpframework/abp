using Riok.Mapperly.Abstractions;
using Volo.Abp.Account.Blazor.Pages.Account;
using Volo.Abp.Mapperly;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Blazor;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class ProfileDtoToPersonalInfoModelMapper : MapperBase<ProfileDto, PersonalInfoModel>
{
    [MapperIgnoreTarget(nameof(PersonalInfoModel.PhoneNumberConfirmed))]
    [MapperIgnoreTarget(nameof(PersonalInfoModel.EmailConfirmed))]
    public override partial PersonalInfoModel Map(ProfileDto source);

    [MapperIgnoreTarget(nameof(PersonalInfoModel.PhoneNumberConfirmed))]
    [MapperIgnoreTarget(nameof(PersonalInfoModel.EmailConfirmed))]
    public override partial void Map(ProfileDto source, PersonalInfoModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class PersonalInfoModelToUpdateProfileDtoMapper : MapperBase<PersonalInfoModel, UpdateProfileDto>
{
    public override partial UpdateProfileDto Map(PersonalInfoModel source);
    public override partial void Map(PersonalInfoModel source, UpdateProfileDto destination);
}