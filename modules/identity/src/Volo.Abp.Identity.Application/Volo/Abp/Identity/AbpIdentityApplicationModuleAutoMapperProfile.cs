using AutoMapper;

namespace Volo.Abp.Identity
{
    public class AbpIdentityApplicationModuleAutoMapperProfile : Profile
    {
        public AbpIdentityApplicationModuleAutoMapperProfile()
        {
            CreateMap<IdentityUser, IdentityUserDto>();
            CreateMap<IdentityRole, IdentityRoleDto>();
            CreateMap<IdentityUser, ProfileDto>();
        }
    }
}