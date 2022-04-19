using AutoMapper;
using Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement.EmailSettingGroup;

namespace Volo.Abp.SettingManagement.Blazor;

public class SettingManagementBlazorAutoMapperProfile : Profile
{
    public SettingManagementBlazorAutoMapperProfile()
    {
        CreateMap<EmailSettingGroupViewComponent.UpdateEmailSettingsViewModel, UpdateEmailSettingsDto>();
        CreateMap<EmailSettingsDto, EmailSettingGroupViewComponent.UpdateEmailSettingsViewModel>();
    }
}
