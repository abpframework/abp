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
            options.CacheProfiles[AbpApiDefinitionController.CacheProfileName] = new CacheProfile
            {
                NoStore = true
            };

            options.CacheProfiles[AbpApplicationLocalizationScriptController.CacheProfileName] = new CacheProfile
            {
                Duration = 30,
                VaryByQueryKeys = new []{ "CultureName" }
            };

            options.CacheProfiles[AbpServiceProxyScriptController.CacheProfileName] = new CacheProfile
            {
                Duration = 90,
                Location = ResponseCacheLocation.Client,
            };

            options.CacheProfiles[AbpApplicationConfigurationScriptController.CacheProfileName] = new CacheProfile
            {
                Duration = 30
            };

            options.CacheProfiles[AbpApplicationLocalizationController.CacheProfileName] = new CacheProfile
            {
                Duration = 30
            };

            options.CacheProfiles[AbpApplicationConfigurationController.CacheProfileName] = new CacheProfile
            {
                Duration = 30
            };
        });
    }

    [Fact]
    public async Task ResponseCache_Test()
    {
        var result = await GetResponseAsync("/api/abp/api-definition");
        result.Headers.GetValues("Cache-Control").ShouldContain("no-store");

        result = await GetResponseAsync("/Abp/ApplicationLocalizationScript?CultureName=en");
        result.Headers.GetValues("Cache-Control").ShouldContain("public, max-age=30");

        result = await GetResponseAsync("/Abp/ApplicationLocalizationScript?CultureName=tr");
        result.Headers.GetValues("Cache-Control").ShouldContain("public, max-age=30");

        result = await GetResponseAsync("/Abp/ServiceProxyScript");
        result.Headers.GetValues("Cache-Control").ShouldContain("max-age=90, private");

        // result = await GetResponseAsync("/Abp/ApplicationConfigurationScript");
        // result.Headers.GetValues("Cache-Control").ShouldContain("public, max-age=30");

        // result = await GetResponseAsync("/api/abp/application-configuration");
        // result.Headers.GetValues("Cache-Control").ShouldContain("public, max-age=30");
    }
}
