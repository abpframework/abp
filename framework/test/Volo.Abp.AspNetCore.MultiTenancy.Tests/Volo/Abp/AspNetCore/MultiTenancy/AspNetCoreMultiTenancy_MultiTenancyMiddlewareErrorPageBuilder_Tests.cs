using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
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
}
