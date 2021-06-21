using AutoMapper;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public class IdentityDomainMappingProfile : Profile
    {
        public IdentityDomainMappingProfile()
        {
            CreateMap<IdentityUser, UserEto>();
            CreateMap<IdentityClaimType, IdentityClaimTypeEto>();
            CreateMap<IdentityRole, IdentityRoleEto>();
            CreateMap<OrganizationUnit, OrganizationUnitEto>();
        }
    }
}