using Shouldly;
using Volo.Abp.BlobStoring.Fakes;
using Volo.Abp.BlobStoring.TestObjects;
using Xunit;

namespace Volo.Abp.BlobStoring;

public class AbpBlobStoringOptions_Tests : AbpBlobStoringTestBase
{
    private readonly IBlobContainerConfigurationProvider _configurationProvider;

    public AbpBlobStoringOptions_Tests()
    {
        _configurationProvider = GetRequiredService<IBlobContainerConfigurationProvider>();
    }

    [Fact]
    public void Should_Property_Set_And_Get_Options_For_Different_Containers()
    {
        var testContainer1Config = _configurationProvider.Get<TestContainer1>();
        testContainer1Config.ProviderType.ShouldBe(typeof(FakeBlobProvider1));
        testContainer1Config.GetConfigurationOrDefault<string>("TestConfig1").ShouldBe("TestValue1");
        testContainer1Config.GetConfigurationOrDefault<string>("TestConfigDefault").ShouldBe("TestValueDefault");

        var testContainer2Config = _configurationProvider.Get<TestContainer2>();
        testContainer2Config.ProviderType.ShouldBe(typeof(FakeBlobProvider2));
        testContainer2Config.GetConfigurationOrNull("TestConfig2").ShouldBe("TestValue2");
        testContainer2Config.GetConfigurationOrNull("TestConfigDefault").ShouldBe("TestValueDefault");
    }

    [Fact]
    public void Should_Fallback_To_Default_ProviderType_When_Not_Explicitly_Configured()
    {
        var config = _configurationProvider.Get<TestContainer3>();
        config.ProviderType.ShouldBe(typeof(FakeBlobProvider1));
        config.IsMultiTenant.ShouldBeFalse();
        config.GetConfigurationOrNull("TestConfigDefault").ShouldBe("TestValueDefault");
    }

    [Fact]
    public void Should_Resolve_Fallback_Chain_Through_Configuration_Provider()
    {
        var testContainer1Config = _configurationProvider.Get<TestContainer1>();
        testContainer1Config.IsMultiTenant.ShouldBeTrue();

        var testContainer3Config = _configurationProvider.Get<TestContainer3>();
        testContainer3Config.IsMultiTenant.ShouldBeFalse();
        testContainer3Config.ProviderType.ShouldBe(typeof(FakeBlobProvider1));
    }
}
