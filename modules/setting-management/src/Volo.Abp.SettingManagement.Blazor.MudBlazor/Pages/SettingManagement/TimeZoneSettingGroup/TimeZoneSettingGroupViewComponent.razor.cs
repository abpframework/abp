using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement.Blazor.MudBlazor.Pages.SettingManagement.TimeZoneSettingGroup;

public partial class TimeZoneSettingGroupViewComponent
{
    [Inject]
    protected ITimeZoneSettingsAppService TimeZoneSettingsAppService { get; set; } = default!;

    [Inject]
    private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; } = default!;

    [Inject]
    protected IUiMessageService UiMessageService { get; set; } = default!;

    [Inject]
    protected ISnackbar Snackbar { get; set; } = default!;

    protected UpdateTimezoneSettingsViewModel? TimezoneSettings;

    public TimeZoneSettingGroupViewComponent()
    {
        ObjectMapperContext = typeof(AbpSettingManagementBlazorMudBlazorModule);
        LocalizationResource = typeof(AbpSettingManagementResource);
    }

    protected async override Task OnInitializedAsync()
    {
        TimezoneSettings = new UpdateTimezoneSettingsViewModel()
        {
            Timezone = await TimeZoneSettingsAppService.GetAsync(),
            TimeZoneItems = await TimeZoneSettingsAppService.GetTimezonesAsync()
        };
    }

    protected virtual async Task OnSelectedValueChangedAsync(string? timezone)
    {
        if (TimezoneSettings != null)
        {
            TimezoneSettings.Timezone = timezone;
        }
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task UpdateSettingsAsync()
    {
        try
        {
            if (TimezoneSettings == null)
            {
                return;
            }

            await TimeZoneSettingsAppService.UpdateAsync(TimezoneSettings.Timezone);
            await CurrentApplicationConfigurationCacheResetService.ResetAsync();
            Snackbar.Add(L["SavedSuccessfully"], Severity.Success);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    public class UpdateTimezoneSettingsViewModel
    {
        public string? Timezone { get; set; }

        public List<NameValue>? TimeZoneItems { get; set; }
    }
}
