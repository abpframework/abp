using AutoMapper;

namespace Volo.Abp.SettingManagement.Web;

public class SettingManagementWebAutoMapperProfile : Profile
{
    public SettingManagementWebAutoMapperProfile()
    {
        CreateMap<EmailSettingsDto, UpdateEmailSettingsDto>();
    }
}