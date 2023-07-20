using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace Volo.Abp.SettingManagement;

[Authorize(SettingManagementPermissions.TimeZone)]
public class TimeZoneSettingsAppService : SettingManagementAppServiceBase, ITimeZoneSettingsAppService
{
    protected ISettingManager SettingManager { get; }
    protected ITimezoneProvider TimezoneProvider { get; }

    public TimeZoneSettingsAppService(ISettingManager settingManager, ITimezoneProvider timezoneProvider)
    {
        SettingManager = settingManager;
        TimezoneProvider = timezoneProvider;
    }

    public virtual async Task<string> GetAsync()
    {
        if (CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host)
        {
            return await SettingManager.GetOrNullGlobalAsync(TimingSettingNames.TimeZone);
        }

        return await SettingManager.GetOrNullForCurrentTenantAsync(TimingSettingNames.TimeZone);
    }

    public virtual Task<List<NameValue>> GetTimezonesAsync()
    {
        return Task.FromResult(TimeZoneHelper.GetTimezones(TimezoneProvider.GetWindowsTimezones()));
    }

    public virtual async Task UpdateAsync(string timezone)
    {
        if (CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host)
        {
            await SettingManager.SetGlobalAsync(TimingSettingNames.TimeZone, timezone);
        }
        else
        {
            await SettingManager.SetForCurrentTenantAsync(TimingSettingNames.TimeZone, timezone);
        }
    }
}
