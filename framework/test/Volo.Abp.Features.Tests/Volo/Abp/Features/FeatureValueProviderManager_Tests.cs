using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.Features;

public class FeatureValueProviderManager_Tests : FeatureTestBase
{
    private readonly IFeatureValueProviderManager _featureValueProviderManager;

    public FeatureValueProviderManager_Tests()
    {
        _featureValueProviderManager = GetRequiredService<IFeatureValueProviderManager>();
    }
    
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.Services.Configure<AbpFeatureOptions>(permissionOptions =>
        {
            permissionOptions.ValueProviders.Add<TestDuplicateFeatureValueProvider>();
        });
    }
    
    [Fact]
    public void Should_Throw_Exception_If_Duplicate_Provider_Name_Detected()
    {
        var exception = Assert.Throws<AbpException>(() =>
        {
            var providers = _featureValueProviderManager.ValueProviders;
        });
        
        exception.Message.ShouldBe($"Duplicate feature value provider name detected: {TestDuplicateFeatureValueProvider.ProviderName}. Providers:{Environment.NewLine}{typeof(TestDuplicateFeatureValueProvider).FullName}{Environment.NewLine}{typeof(DefaultValueFeatureValueProvider).FullName}");
    }
}

public class TestDuplicateFeatureValueProvider : FeatureValueProvider
{
    public const string ProviderName = "D";

    public override string Name => ProviderName;

    public TestDuplicateFeatureValueProvider(IFeatureStore settingStore)
        : base(settingStore)
    {

    }

    public override Task<string> GetOrNullAsync(FeatureDefinition setting)
    {
        throw new NotImplementedException();
    }
}