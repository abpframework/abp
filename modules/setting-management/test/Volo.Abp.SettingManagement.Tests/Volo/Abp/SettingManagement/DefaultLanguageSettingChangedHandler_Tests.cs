using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.SettingManagement;

public class DefaultLanguageSettingChangedHandler_Tests : SettingsTestBase
{
    private readonly ISettingManagementStore _settingManagementStore;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public DefaultLanguageSettingChangedHandler_Tests()
    {
        _settingManagementStore = GetRequiredService<ISettingManagementStore>();
        _distributedEventBus = GetRequiredService<IDistributedEventBus>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task Should_Publish_LanguageChangedEto_When_DefaultLanguage_Setting_Changed()
    {
        var languageChanged = false;

        using (var uow = _unitOfWorkManager.Begin())
        {
            _distributedEventBus.Subscribe<LanguageChangedEto>(_ =>
            {
                languageChanged = true;
                return Task.CompletedTask;
            });

            await _settingManagementStore.SetAsync(
                LocalizationSettingNames.DefaultLanguage,
                "tr;tr",
                GlobalSettingValueProvider.ProviderName,
                null);

            await uow.CompleteAsync();
        }

        languageChanged.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Not_Publish_LanguageChangedEto_When_Other_Setting_Changed()
    {
        var languageChanged = false;

        using (var uow = _unitOfWorkManager.Begin())
        {
            _distributedEventBus.Subscribe<LanguageChangedEto>(_ =>
            {
                languageChanged = true;
                return Task.CompletedTask;
            });

            await _settingManagementStore.SetAsync(
                "MySetting2",
                "MySetting2Value-updated",
                GlobalSettingValueProvider.ProviderName,
                null);

            await uow.CompleteAsync();
        }

        languageChanged.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Not_Publish_LanguageChangedEto_When_User_DefaultLanguage_Setting_Changed()
    {
        var languageChanged = false;

        using (var uow = _unitOfWorkManager.Begin())
        {
            _distributedEventBus.Subscribe<LanguageChangedEto>(_ =>
            {
                languageChanged = true;
                return Task.CompletedTask;
            });

            await _settingManagementStore.SetAsync(
                LocalizationSettingNames.DefaultLanguage,
                "tr;tr",
                UserSettingValueProvider.ProviderName,
                Guid.NewGuid().ToString());

            await uow.CompleteAsync();
        }

        languageChanged.ShouldBeFalse();
    }
}
