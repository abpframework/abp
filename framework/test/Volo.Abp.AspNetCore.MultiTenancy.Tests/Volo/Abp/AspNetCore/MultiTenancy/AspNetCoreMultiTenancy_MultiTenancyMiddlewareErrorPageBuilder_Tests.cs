using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Http;
using Xunit;

namespace Volo.Abp.AspNetCore.MultiTenancy;

public class AspNetCoreMultiTenancy_MultiTenancyMiddlewareErrorPageBuilder_Tests : AspNetCoreMultiTenancyTestBase
{
    private readonly AbpAspNetCoreMultiTenancyOptions _options;

    public AspNetCoreMultiTenancy_MultiTenancyMiddlewareErrorPageBuilder_Tests()
    {
        _options = ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreMultiTenancyOptions>>().Value;
    }

    [Fact]
    public async Task MultiTenancyMiddlewareErrorPageBuilder()
    {
        var result = await GetResponseAsStringAsync($"http://abp.io?{_options.TenantKey}=<script>alert(hi)</script>", HttpStatusCode.NotFound);
        result.ShouldNotContain("<script>alert(hi)</script>");
    }

    [Fact]
    public async Task MultiTenancyMiddlewareErrorPageBuilder_Ajax_Test()
    {
        using (var response = await GetResponseAsync($"http://abp.io?{_options.TenantKey}=abpio", HttpStatusCode.NotFound, xmlHttpRequest: true))
        {
            var result = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<RemoteServiceErrorResponse>(result, new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            error.Error.ShouldNotBeNull();
            error.Error.Message.ShouldBe("Tenant not found!");
            error.Error.Details.ShouldBe("There is no tenant with the tenant id or name: abpio");
        }
    }
}
