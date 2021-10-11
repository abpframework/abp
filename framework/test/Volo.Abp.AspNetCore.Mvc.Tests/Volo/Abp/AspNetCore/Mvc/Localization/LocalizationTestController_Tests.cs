using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    public class LocalizationTestController_Tests : AspNetCoreMvcTestBase
    {
        class TestRequestCultureProvider : RequestCultureProvider
        {
            public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
            {
                return Task.FromResult(new ProviderCultureResult((StringSegment) "tr", (StringSegment) "hu"));
            }
        }

        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Configure<AbpRequestLocalizationOptions>(options =>
            {
                options.RequestLocalizationOptionConfigurators.Add((serviceProvider, localizationOptions) =>
                {
                    localizationOptions.RequestCultureProviders.Insert(0, new TestRequestCultureProvider());
                    return Task.CompletedTask;
                });
            });
        }

        [Fact]
        public async Task TestRequestCultureProvider_Test()
        {
            var response = await GetResponseAsync("api/LocalizationTestController", HttpStatusCode.OK);
            var resultAsString = await response.Content.ReadAsStringAsync();
            resultAsString.ToLower().ShouldBe("tr:hu");
        }
    }
}
