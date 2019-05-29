using AutoMapper;

namespace Volo.Abp.Identity
{
    public class IdentityApplicationModuleAutoMapperProfile : Profile
    {
        public IdentityApplicationModuleAutoMapperProfile()
        {
            CreateMap<IdentityUser, IdentityUserDto>();
            CreateMap<IdentityRole, IdentityRoleDto>();
        }
    }
}