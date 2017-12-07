using AutoMapper;

namespace Volo.Abp.Identity.ObjectMappings
{
    public class AbpIdentityApplicationModuleAutoMapperProfile : Profile
    {
        public AbpIdentityApplicationModuleAutoMapperProfile()
        {
            CreateMap<IdentityUser, IdentityUserDto>();
            CreateMap<IdentityRole, IdentityRoleDto>();
            CreateMap<IdentityRoleDto, IdentityUserRoleDto>(); //TODO: This should be removed!
        }
    }
}