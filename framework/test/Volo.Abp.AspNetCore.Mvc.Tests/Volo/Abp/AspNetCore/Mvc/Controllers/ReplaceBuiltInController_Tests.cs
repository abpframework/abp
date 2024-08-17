using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Controllers;

public class ReplaceBuiltInController_Tests : AspNetCoreMvcTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ControllersToRemove.Add(typeof(AbpLanguagesController));
        });
    }

    [Fact]
    public async Task Test()
    {
        var random = Guid.NewGuid().ToString("N");

        (await GetResponseAsObjectAsync<MyApplicationConfigurationDto>("api/abp/application-configuration?random=" + random)).Random.ShouldBe(random);
        (await GetResponseAsObjectAsync<MyApplicationLocalizationDto>("api/abp/application-localization?CultureName=en&random=" + random)).Random.ShouldBe(random);

        (await GetResponseAsync("Abp/Languages/Switch", HttpStatusCode.NotFound)).StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
