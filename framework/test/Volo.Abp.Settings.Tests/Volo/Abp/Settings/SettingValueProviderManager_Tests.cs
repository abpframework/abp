using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute.Extensions;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Settings;

public class SettingValueProviderManager_Tests: AbpIntegratedTest<AbpSettingsTestModule>
{
    private readonly ISettingValueProviderManager _settingValueProviderManager;

    public SettingValueProviderManager_Tests()
    {
        _settingValueProviderManager = GetRequiredService<ISettingValueProviderManager>();
    }
    
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
        options.Services.Configure<AbpSettingOptions>(settingOptions =>
        {
            settingOptions.ValueProviders.Add<Test2SettingValueProvider>();
        });
    }
    
    [Fact]
    public void Should_Throw_Exception_If_Duplicate_Provider_Name_Detected()
    {
        var exception = Assert.Throws<AbpException>(() =>
        {
            var providers = _settingValueProviderManager.Providers;
        });
        
        exception.Message.ShouldBe($"Duplicate setting value provider name detected: Test. Providers:{Environment.NewLine}Volo.Abp.Settings.Test2SettingValueProvider{Environment.NewLine}Volo.Abp.Settings.TestSettingValueProvider");
    }
}

public class Test2SettingValueProvider : ISettingValueProvider, ITransientDependency
{
    public const string ProviderName = "Test";


    public string Name => ProviderName;

    public Test2SettingValueProvider()
    {
    }

    public Task<string> GetOrNullAsync(SettingDefinition setting)
    {
        throw new NotImplementedException();
    }

    public Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
    {
        throw new NotImplementedException();
    }
}