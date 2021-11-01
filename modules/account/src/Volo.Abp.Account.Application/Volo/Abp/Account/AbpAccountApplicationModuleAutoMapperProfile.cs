using AutoMapper;
using Volo.Abp.Identity;

namespace Volo.Abp.Account
{
    public class AbpAccountApplicationModuleAutoMapperProfile : Profile
    {
        public AbpAccountApplicationModuleAutoMapperProfile()
        {
            CreateMap<IdentityUser, ProfileDto>()
                .ForMember(dest => dest.HasPassword,
                    op => op.MapFrom(src => src.PasswordHash != null))
                .MapExtraProperties();
        }
    }
}
