using AutoMapper;

namespace Volo.Abp.Identity.Blazor
{
    public class AbpIdentityBlazorAutoMapperProfile : Profile
    {
        public AbpIdentityBlazorAutoMapperProfile()
        {
            CreateMap<IdentityRoleDto, IdentityRoleUpdateDto>()
                .MapExtraProperties();
        }
    }
}
