using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.SettingManagement;

[RemoteService(Name = SettingManagementRemoteServiceConsts.RemoteServiceName)]
[Area(SettingManagementRemoteServiceConsts.ModuleName)]
[Route("api/setting-management/timezone")]
public class TimeZoneSettingsController : AbpControllerBase, ITimeZoneSettingsAppService
{
    private readonly ITimeZoneSettingsAppService _timeZoneSettingsAppService;

    public TimeZoneSettingsController(ITimeZoneSettingsAppService timeZoneSettingsAppService)
    {
        _timeZoneSettingsAppService = timeZoneSettingsAppService;
    }

    [HttpGet]
    public Task<string> GetAsync()
    {
        return _timeZoneSettingsAppService.GetAsync();
    }

    [HttpGet]
    [Route("timezones")]
    public Task<List<NameValue>> GetTimezonesAsync()
    {
        return _timeZoneSettingsAppService.GetTimezonesAsync();
    }

    [HttpPost]
    public Task UpdateAsync(string timezone)
    {
        return _timeZoneSettingsAppService.UpdateAsync(timezone);
    }
}
