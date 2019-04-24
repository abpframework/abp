using AutoMapper;
using Volo.Abp.AutoMapper;

namespace OrganizationService
{
    public class OrganizationServiceApplicationAutoMapperProfile : Profile
    {
        public OrganizationServiceApplicationAutoMapperProfile()
        {
            CreateMap<AbpOrganization, AbpOrganizationDto>();
            CreateMap<CreateUpdateAbpOrganizationDto, AbpOrganization>();
            
        }
    }
}