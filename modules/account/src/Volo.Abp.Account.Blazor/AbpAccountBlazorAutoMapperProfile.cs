using AutoMapper;
using Volo.Abp.Account.Blazor.Pages.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Blazor;

public class AbpAccountBlazorAutoMapperProfile : Profile
{
    public AbpAccountBlazorAutoMapperProfile()
    {
        CreateMap<ProfileDto, PersonalInfoModel>()
            .MapExtraProperties()
            .Ignore(x => x.PhoneNumberConfirmed)
            .Ignore(x => x.EmailConfirmed);

        CreateMap<PersonalInfoModel, UpdateProfileDto>()
            .MapExtraProperties();
    }
}
