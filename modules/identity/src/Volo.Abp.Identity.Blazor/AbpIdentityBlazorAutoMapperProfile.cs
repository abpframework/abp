using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Volo.Abp.Identity.Blazor
{
    public class AbpIdentityBlazorAutoMapperProfile : Profile
    {
        public AbpIdentityBlazorAutoMapperProfile()
        {
            CreateMap<IdentityUserDto, IdentityUserUpdateDto>()
                .MapExtraProperties()
                .Ignore(x => x.Password)
                .Ignore(x => x.RoleNames);
            
            CreateMap<IdentityRoleDto, IdentityRoleUpdateDto>()
                .MapExtraProperties();

            CreateMap<IdentityUserDto, IdentityUserUpdateDto>()
                .Ignore(x => x.Password)
                .Ignore(x => x.RoleNames)
                .MapExtraProperties();
        }
    }
}
