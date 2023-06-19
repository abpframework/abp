using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Timing.Localization.Resources.AbpTiming;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.TimeZoneSettingGroup;

public class TimeZoneSettingGroupViewComponent : AbpViewComponent
{
    protected ITimeZoneSettingsAppService TimeZoneSettingsAppService { get; }

    public TimeZoneSettingGroupViewComponent(ITimeZoneSettingsAppService timeZoneSettingsAppService)
    {
        ObjectMapperContext = typeof(AbpSettingManagementWebModule);
        TimeZoneSettingsAppService = timeZoneSettingsAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var timezone = await TimeZoneSettingsAppService.GetAsync();
        var timezones = await TimeZoneSettingsAppService.GetTimezonesAsync();
        var model = new UpdateTimezoneSettingsViewModel()
        {
            Timezone = timezone,
            TimeZoneItems = new List<SelectListItem>()
        };
        model.TimeZoneItems.AddRange(timezones.Select(x => new SelectListItem(x.Name, x.Value)).ToList());
        return View("~/Pages/SettingManagement/Components/TimeZoneSettingGroup/Default.cshtml", model);
    }

    public class UpdateTimezoneSettingsViewModel
    {
        public string Timezone { get; set; }

        public List<SelectListItem> TimeZoneItems { get; set; }
    }
}
