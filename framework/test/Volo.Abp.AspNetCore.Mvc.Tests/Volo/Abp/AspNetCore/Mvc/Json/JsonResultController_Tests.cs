using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.Json;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Json;

public class JsonResultController_Tests : AspNetCoreMvcTestBase
{
    protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<AbpJsonOptions>(options =>
        {
            options.DefaultDateTimeFormat = "yyyy*MM*dd";
        });

        base.ConfigureServices(context, services);
    }

    [Fact]
    public async Task DateFormatString_Test()
    {
        var time = await GetResponseAsStringAsync(
            "/api/json-result-test/json-result-action"
        );

        time.ShouldContain("2019*01*01");
    }
}
