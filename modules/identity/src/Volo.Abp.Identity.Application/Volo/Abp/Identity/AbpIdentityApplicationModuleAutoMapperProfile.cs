using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Volo.Abp.Identity
{
    public class AbpIdentityApplicationModuleAutoMapperProfile : Profile
    {
        public AbpIdentityApplicationModuleAutoMapperProfile()
        {
            CreateMap<IdentityUser, IdentityUserDto>()
                .MapExtraProperties();

            CreateMap<IdentityRole, IdentityRoleDto>()
                .MapExtraProperties();

            CreateMap<IdentityUser, ProfileDto>()
                .Ignore(x=>x.IsExternalLoggedIn)
                .MapExtraProperties();
        }
    }
}
