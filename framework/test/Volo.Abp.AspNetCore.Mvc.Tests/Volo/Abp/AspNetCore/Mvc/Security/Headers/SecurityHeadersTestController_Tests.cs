using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.AspNetCore.Security;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Security.Headers;

public class SecurityHeadersTestController_Tests : AspNetCoreMvcTestBase
{
    protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<AbpSecurityHeadersOptions>(options =>
        {
            options.UseContentSecurityPolicyHeader = true;
        });

        base.ConfigureServices(context, services);
    }

    [Fact]
    public async Task SecurityHeaders_Should_Be_Added()
    {
        var responseMessage = await GetResponseAsync("/SecurityHeadersTest/Get");
        responseMessage.Headers.ShouldContain(x => x.Key == "X-Content-Type-Options" & x.Value.First().ToString() == "nosniff");
        responseMessage.Headers.ShouldContain(x => x.Key == "X-XSS-Protection" & x.Value.First().ToString() == "1; mode=block");
        responseMessage.Headers.ShouldContain(x => x.Key == "X-Frame-Options" & x.Value.First().ToString() == "SAMEORIGIN");
        responseMessage.Headers.ShouldContain(x => x.Key == "X-Content-Type-Options" & x.Value.First().ToString() == "nosniff");
        responseMessage.Headers.ShouldContain(x => x.Key == "Content-Security-Policy" & x.Value.First().ToString() == "object-src 'none'; form-action 'self'; frame-ancestors 'none'");
    }
}
