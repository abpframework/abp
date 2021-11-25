using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web.Pages.Identity.Roles;
using CreateUserModalModel = Volo.Abp.Identity.Web.Pages.Identity.Users.CreateModalModel;
using EditUserModalModel = Volo.Abp.Identity.Web.Pages.Identity.Users.EditModalModel;

namespace Volo.Abp.Identity.Web;

public class AbpIdentityWebAutoMapperProfile : Profile
{
    public AbpIdentityWebAutoMapperProfile()
    {
        CreateUserMappings();
        CreateRoleMappings();
    }

    protected virtual void CreateUserMappings()
    {
        //List
        CreateMap<IdentityUserDto, EditUserModalModel.UserInfoViewModel>()
            .Ignore(x => x.Password);

        //CreateModal
        CreateMap<CreateUserModalModel.UserInfoViewModel, IdentityUserCreateDto>()
            .MapExtraProperties()
            .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

        CreateMap<IdentityRoleDto, CreateUserModalModel.AssignedRoleViewModel>()
            .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());

        //EditModal
        CreateMap<EditUserModalModel.UserInfoViewModel, IdentityUserUpdateDto>()
            .MapExtraProperties()
            .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

        CreateMap<IdentityRoleDto, EditUserModalModel.AssignedRoleViewModel>()
            .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());
    }

    protected virtual void CreateRoleMappings()
    {
        //List
        CreateMap<IdentityRoleDto, EditModalModel.RoleInfoModel>();

        //CreateModal
        CreateMap<CreateModalModel.RoleInfoModel, IdentityRoleCreateDto>()
            .MapExtraProperties();

        //EditModal
        CreateMap<EditModalModel.RoleInfoModel, IdentityRoleUpdateDto>()
            .MapExtraProperties();
    }
}
