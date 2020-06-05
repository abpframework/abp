using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using AutoMapper;

namespace Volo.Abp.Account.Web
{
    public class AbpAccountWebAutoMapperProfile : Profile
    {
        public AbpAccountWebAutoMapperProfile()
        {
            CreateMap<ProfileDto, PersonalSettingsInfoModel>();
        }
    }
}
