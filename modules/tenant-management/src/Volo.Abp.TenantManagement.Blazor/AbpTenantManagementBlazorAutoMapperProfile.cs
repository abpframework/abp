using AutoMapper;

namespace Volo.Abp.TenantManagement.Blazor
{
    public class AbpTenantManagementBlazorAutoMapperProfile : Profile
    {
        public AbpTenantManagementBlazorAutoMapperProfile()
        {
            CreateMap<TenantDto, TenantUpdateDto>()
                .MapExtraProperties();
        }
    }
}
