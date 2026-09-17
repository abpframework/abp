using Riok.Mapperly.Abstractions;
using Volo.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;
using Volo.Abp.Mapperly;

namespace Volo.Abp.Account.Web;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class ProfileDtoToPersonalInfoModelMapper : MapperBase<ProfileDto, AccountProfilePersonalInfoManagementGroupViewComponent.PersonalInfoModel>
{
    public override partial AccountProfilePersonalInfoManagementGroupViewComponent.PersonalInfoModel Map(ProfileDto source);
    public override partial void Map(ProfileDto source, AccountProfilePersonalInfoManagementGroupViewComponent.PersonalInfoModel destination);
}