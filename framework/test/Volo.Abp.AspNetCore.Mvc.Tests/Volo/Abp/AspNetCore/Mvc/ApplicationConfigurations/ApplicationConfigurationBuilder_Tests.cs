using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class ApplicationConfigurationBuilder_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task ApplicationConfigurationBuilder_GetAsync()
    {
        var applicationConfigurationBuilder = GetRequiredService<IAbpApplicationConfigurationAppService>();

        var config = await applicationConfigurationBuilder.GetAsync(new ApplicationConfigurationRequestOptions());

        config.Auth.ShouldNotBeNull();
        config.Localization.ShouldNotBeNull();
        config.GetProperty("TestKey").ShouldBe("TestValue");
    }
}
