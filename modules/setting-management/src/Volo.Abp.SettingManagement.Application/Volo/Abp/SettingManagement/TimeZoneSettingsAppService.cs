using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeZoneConverter;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Timing;

namespace Volo.Abp.SettingManagement;

//[RequiresFeature(SettingManagementFeatures.EnableTimeZone)]
public class TimeZoneSettingsAppService : SettingManagementAppServiceBase, ITimeZoneSettingsAppService
{
    protected ISettingManager SettingManager { get; }
    protected ISettingProvider SettingProvider { get; }
    protected ITimezoneProvider TimezoneProvider { get; }

    public TimeZoneSettingsAppService(ISettingManager settingManager, ISettingProvider settingProvider, ITimezoneProvider timezoneProvider)
    {
        SettingManager = settingManager;
        SettingProvider = settingProvider;
        TimezoneProvider = timezoneProvider;
    }

    public virtual async Task<string> GetAsync()
    {
        var timezone = await SettingProvider.GetOrNullAsync(TimingSettingNames.TimeZone);
        return timezone ?? "UTC";
    }

    public virtual Task<List<NameValue>> GetTimezonesAsync()
    {
        var timezones = TimezoneProvider.GetWindowsTimezones()
            .OrderBy(x => x.Name)
            .Select(x => new NameValue( $"{x.Name} ({GetTimezoneOffset(TZConvert.GetTimeZoneInfo(x.Name))})", x.Name))
            .ToList();

        return Task.FromResult(timezones);
    }

    protected virtual string GetTimezoneOffset(TimeZoneInfo timeZoneInfo)
    {
        if (timeZoneInfo.BaseUtcOffset < TimeSpan.Zero)
        {
            return "-" + timeZoneInfo.BaseUtcOffset.ToString(@"hh\:mm");
        }

        return "+" + timeZoneInfo.BaseUtcOffset.ToString(@"hh\:mm");
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
