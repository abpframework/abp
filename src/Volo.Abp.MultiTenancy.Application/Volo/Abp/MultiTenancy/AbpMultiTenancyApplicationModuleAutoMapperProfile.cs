using AutoMapper;

namespace Volo.Abp.MultiTenancy
{
    public class AbpMultiTenancyApplicationModuleAutoMapperProfile : Profile
    {
        public AbpMultiTenancyApplicationModuleAutoMapperProfile()
        {
            CreateMap<Tenant, TenantDto>();
        }
    }
}