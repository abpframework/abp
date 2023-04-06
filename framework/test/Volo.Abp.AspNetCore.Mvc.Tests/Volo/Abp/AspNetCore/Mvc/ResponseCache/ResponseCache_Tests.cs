using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.ProxyScripting;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ResponseCache;

public class ResponseCache_Tests : AspNetCoreMvcTestBase
{
    protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<MvcOptions>(options =>
        {
            // Customize the cache profile.
            options.CacheProfiles[AbpServiceProxyScriptController.GetAllCacheProfileName] = new CacheProfile
            {
                Duration = 120,
                Location = ResponseCacheLocation.Client
            };
        });
    }

    [Fact]
    public async Task ResponseCache_Test()
    {
        var result = await GetResponseAsync("/Abp/ApplicationLocalizationScript?CultureName=en");
        result.Headers.GetValues("Cache-Control").ShouldContain("public, max-age=60");

        result = await GetResponseAsync("/Abp/ApplicationLocalizationScript?CultureName=tr");
        result.Headers.GetValues("Cache-Control").ShouldContain("public, max-age=60");

        // result = await GetResponseAsync("/Abp/ApplicationConfigurationScript");
        // result.Headers.GetValues("Cache-Control").ShouldContain("public, max-age=60");

        result = await GetResponseAsync("/Abp/ServiceProxyScript");
        result.Headers.GetValues("Cache-Control").ShouldContain("max-age=120, private");
    }
}
