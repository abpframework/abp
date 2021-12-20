using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class ApplicationConfigurationBuilder_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task ApplicationConfigurationBuilder_GetAsync()
    {
        var applicationConfigurationBuilder = GetRequiredService<IAbpApplicationConfigurationAppService>();

        var config = await applicationConfigurationBuilder.GetAsync();

        config.Auth.ShouldNotBeNull();
        config.Localization.ShouldNotBeNull();
    }
}
