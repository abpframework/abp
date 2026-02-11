using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Abp.Identity.Web.Pages.Identity.Roles;
using CreateUserModalModel = Volo.Abp.Identity.Web.Pages.Identity.Users.CreateModalModel;
using EditUserModalModel = Volo.Abp.Identity.Web.Pages.Identity.Users.EditModalModel;

namespace Volo.Abp.Identity.Web;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityUserDtoToEditUserModalModelUserInfoViewModelMapper : MapperBase<IdentityUserDto, EditUserModalModel.UserInfoViewModel>
{
    [MapperIgnoreTarget(nameof(EditUserModalModel.UserInfoViewModel.Password))]
    public override partial EditUserModalModel.UserInfoViewModel Map(IdentityUserDto source);

    [MapperIgnoreTarget(nameof(EditUserModalModel.UserInfoViewModel.Password))]
    public override partial void Map(IdentityUserDto source, EditUserModalModel.UserInfoViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CreateUserModalModelUserInfoViewModelToIdentityUserCreateDtoMapper : MapperBase<CreateUserModalModel.UserInfoViewModel, IdentityUserCreateDto>
{
    [MapperIgnoreTarget(nameof(IdentityUserCreateDto.RoleNames))]
    public override partial IdentityUserCreateDto Map(CreateUserModalModel.UserInfoViewModel source);

    [MapperIgnoreTarget(nameof(IdentityUserCreateDto.RoleNames))]
    public override partial void Map(CreateUserModalModel.UserInfoViewModel source, IdentityUserCreateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class IdentityRoleDtoToCreateUserModalModelAssignedRoleViewModelMapper : MapperBase<IdentityRoleDto, CreateUserModalModel.AssignedRoleViewModel>
{
    [MapperIgnoreTarget(nameof(CreateUserModalModel.AssignedRoleViewModel.IsAssigned))]
    public override partial CreateUserModalModel.AssignedRoleViewModel Map(IdentityRoleDto source);

    [MapperIgnoreTarget(nameof(CreateUserModalModel.AssignedRoleViewModel.IsAssigned))]
    public override partial void Map(IdentityRoleDto source, CreateUserModalModel.AssignedRoleViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EditUserModalModelUserInfoViewModelToIdentityUserUpdateDtoMapper : MapperBase<EditUserModalModel.UserInfoViewModel, IdentityUserUpdateDto>
{
    [MapperIgnoreTarget(nameof(IdentityUserUpdateDto.RoleNames))]
    public override partial IdentityUserUpdateDto Map(EditUserModalModel.UserInfoViewModel source);

    [MapperIgnoreTarget(nameof(IdentityUserUpdateDto.RoleNames))]
    public override partial void Map(EditUserModalModel.UserInfoViewModel source, IdentityUserUpdateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityRoleDtoToEditUserModalModelAssignedRoleViewModelMapper : MapperBase<IdentityRoleDto, EditUserModalModel.AssignedRoleViewModel>
{
    [MapperIgnoreTarget(nameof(EditUserModalModel.AssignedRoleViewModel.IsAssigned))]
    public override partial EditUserModalModel.AssignedRoleViewModel Map(IdentityRoleDto source);

    [MapperIgnoreTarget(nameof(EditUserModalModel.AssignedRoleViewModel.IsAssigned))]
    public override partial void Map(IdentityRoleDto source, EditUserModalModel.AssignedRoleViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityUserDtoToEditUserModalModelDetailViewModelMapper : MapperBase<IdentityUserDto, EditUserModalModel.DetailViewModel>
{
    [MapperIgnoreTarget(nameof(EditUserModalModel.DetailViewModel.CreatedBy))]
    [MapperIgnoreTarget(nameof(EditUserModalModel.DetailViewModel.ModifiedBy))]
    public override partial EditUserModalModel.DetailViewModel Map(IdentityUserDto source);

    [MapperIgnoreTarget(nameof(EditUserModalModel.DetailViewModel.CreatedBy))]
    [MapperIgnoreTarget(nameof(EditUserModalModel.DetailViewModel.ModifiedBy))]
    public override partial void Map(IdentityUserDto source, EditUserModalModel.DetailViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityRoleDtoToEditModalModelRoleInfoModelMapper : MapperBase<IdentityRoleDto, EditModalModel.RoleInfoModel>
{
    public override partial EditModalModel.RoleInfoModel Map(IdentityRoleDto source);
    public override partial void Map(IdentityRoleDto source, EditModalModel.RoleInfoModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CreateModalModelRoleInfoModelToIdentityRoleCreateDtoMapper : MapperBase<CreateModalModel.RoleInfoModel, IdentityRoleCreateDto>
{
    public override partial IdentityRoleCreateDto Map(CreateModalModel.RoleInfoModel source);
    public override partial void Map(CreateModalModel.RoleInfoModel source, IdentityRoleCreateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class EditModalModelRoleInfoModelToIdentityRoleUpdateDtoMapper : MapperBase<EditModalModel.RoleInfoModel, IdentityRoleUpdateDto>
{
    public override partial IdentityRoleUpdateDto Map(EditModalModel.RoleInfoModel source);
    public override partial void Map(EditModalModel.RoleInfoModel source, IdentityRoleUpdateDto destination);
}
