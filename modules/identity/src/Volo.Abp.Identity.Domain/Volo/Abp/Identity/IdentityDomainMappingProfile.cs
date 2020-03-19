using AutoMapper;

namespace Volo.Abp.Identity
{
    public class IdentityDomainMappingProfile : Profile
    {
        public IdentityDomainMappingProfile()
        {
            CreateMap<IdentityClaimType, IdentityClaimTypeEto>();
            CreateMap<IdentityRoleEto, IdentityRoleEto>();
        }
    }
}