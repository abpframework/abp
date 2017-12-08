using AutoMapper;
using Volo.Abp.Identity.Web.Pages.Identity.Users;

namespace Volo.Abp.Identity.Web.ObjectMappings
{
    public class AbpIdentityWebAutoMapperProfile : Profile
    {
        public AbpIdentityWebAutoMapperProfile()
        {
            //EditModal

            CreateMap<IdentityUserDto, EditModalModel.UserInfoViewModel>();

            CreateMap<EditModalModel.UserInfoViewModel, IdentityUserUpdateDto>()
                .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

            CreateMap<IdentityRoleDto, EditModalModel.AssignedRoleViewModel>()
                .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());

            //CreateModal

            CreateMap<CreateModalModel.UserInfoViewModel, IdentityUserCreateDto>()
                .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

            CreateMap<IdentityRoleDto, CreateModalModel.AssignedRoleViewModel>()
                .ForMember(dest => dest.IsAssigned, opt => opt.Ignore());
        }
    }
}
