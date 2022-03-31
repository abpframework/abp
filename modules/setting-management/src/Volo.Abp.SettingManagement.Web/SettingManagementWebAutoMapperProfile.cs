using AutoMapper;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.EmailSettingGroup;

namespace Volo.Abp.SettingManagement.Web;

public class SettingManagementWebAutoMapperProfile : Profile
{
    public SettingManagementWebAutoMapperProfile()
    {
        CreateMap<EmailSettingsDto, EmailSettingGroupViewComponent.UpdateEmailSettingsViewModel>();
    }
}