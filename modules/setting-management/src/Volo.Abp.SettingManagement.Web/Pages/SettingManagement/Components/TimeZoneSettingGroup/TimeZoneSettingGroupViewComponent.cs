using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.TimeZoneSettingGroup;

public class TimeZoneSettingGroupViewComponent : AbpViewComponent
{
    public TimeZoneSettingGroupViewComponent(ITimeZoneSettingsAppService timeZoneSettingsAppService)
    {
        ObjectMapperContext = typeof(AbpSettingManagementWebModule);
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        return View("~/Pages/SettingManagement/Components/TimeZoneSettingGroup/Default.cshtml");
    }
}
