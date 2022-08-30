using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Localization;

public class AbpStringLocalizerFactory_Tests : AbpIntegratedTest<AbpLocalizationTestModule>
{
    private readonly IStringLocalizerFactory _factory;

    public AbpStringLocalizerFactory_Tests()
    {
        _factory = GetRequiredService<IStringLocalizerFactory>();
    }

    [Fact]
    public void Factory_Type_Should_Be_AbpStringLocalizerFactory()
    {
        ProxyHelper.UnProxy(_factory).ShouldBeOfType<AbpStringLocalizerFactory>();
    }
    
    [Fact]
    public void Should_Create_Resource_By_Name()
    {
        using (CultureHelper.Use("en"))
        {
            var localizer = _factory.CreateByResourceNameOrNull("Test");
            localizer.ShouldNotBeNull();
            localizer["CarPlural"].Value.ShouldBe("Cars");
        }
    }
    
    [Fact]
    public async Task Should_Create_Resource_By_Name_Async()
    {
        using (CultureHelper.Use("en"))
        {
            var localizer = await _factory.CreateByResourceNameOrNullAsync("Test");
            localizer.ShouldNotBeNull();
            localizer["CarPlural"].Value.ShouldBe("Cars");
        }
    }

    [Fact]
    public void Should_Throw_Exception_For_Unknown_Resource_Names()
    {
        Assert.Throws<AbpException>(
            () => _factory.CreateByResourceName("UnknownResourceName")
        );
    }

    [Fact]
    public async Task Should_Throw_Exception_For_Unknown_Resource_Names_Async()
    {
        await Assert.ThrowsAsync<AbpException>(
            async () => await _factory.CreateByResourceNameAsync("UnknownResourceName")
        );
    }
}