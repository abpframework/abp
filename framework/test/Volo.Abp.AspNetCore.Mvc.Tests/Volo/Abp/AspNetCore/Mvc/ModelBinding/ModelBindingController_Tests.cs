using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.Http;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ModelBinding;

public abstract class ModelBindingController_Tests : AspNetCoreMvcTestBase
{
    protected DateTimeKind Kind { get; set; }

    protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.UnsupportedTypes.Add<GetDateTimeKindModel>();
            options.UnsupportedTypes.Add<GetDateTimeKindModel.GetDateTimeKindInnerModel>();
        });
    }

    [Fact]
    public async Task DateTimeKind_Test()
    {
        var response = await Client.GetAsync("/api/model-Binding-test/DateTimeKind?input=2010-01-01T00:00:00Z");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var resultAsString = await response.Content.ReadAsStringAsync();
        resultAsString.ShouldBe(Kind.ToString().ToLower());
    }

    [Fact]
    public async Task NullableDateTimeKind_Test()
    {
        var response =
            await Client.GetAsync("/api/model-Binding-test/NullableDateTimeKind?input=2010-01-01T00:00:00Z");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var resultAsString = await response.Content.ReadAsStringAsync();
        resultAsString.ShouldBe(Kind.ToString().ToLower());
    }

    [Fact]
    public async Task DisableDateTimeNormalizationDateTimeKind_Test()
    {
        var response =
            await Client.GetAsync(
                "/api/model-Binding-test/DisableDateTimeNormalizationDateTimeKind?input=2010-01-01T00:00:00Z");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var resultAsString = await response.Content.ReadAsStringAsync();
        //Time parameter(2010-01-01T00:00:00Z) with time zone information, so the default Kind is UTC
        //https://docs.microsoft.com/en-us/aspnet/core/migration/31-to-50?view=aspnetcore-3.1&tabs=visual-studio#datetime-values-are-model-bound-as-utc-times
        resultAsString.ShouldBe(DateTimeKind.Utc.ToString().ToLower());
    }

    [Fact]
    public async Task DisableDateTimeNormalizationNullableDateTimeKind_Test()
    {
        var response =
            await Client.GetAsync(
                "/api/model-Binding-test/DisableDateTimeNormalizationNullableDateTimeKind?input=2010-01-01T00:00:00Z");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var resultAsString = await response.Content.ReadAsStringAsync();
        //Time parameter(2010-01-01T00:00:00Z) with time zone information, so the default Kind is UTC
        //https://docs.microsoft.com/en-us/aspnet/core/migration/31-to-50?view=aspnetcore-3.1&tabs=visual-studio#datetime-values-are-model-bound-as-utc-times
        resultAsString.ShouldBe(DateTimeKind.Utc.ToString().ToLower());
    }

    [Fact]
    public async Task ComplexTypeDateTimeKind_Test()
    {
        var response = await Client.GetAsync("/api/model-Binding-test/ComplexTypeDateTimeKind?" +
                                             "Time1=2010-01-01T00:00:00Z&" +
                                             "Time2=2010-01-01T00:00:00Z&" +
                                             "Time3=2010-01-01T00:00:00Z&" +
                                             "InnerModel.Time4=2010-01-01T00:00:00Z");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var resultAsString = await response.Content.ReadAsStringAsync();
        //Time parameter(2010-01-01T00:00:00Z) with time zone information, so the default Kind is UTC
        //https://docs.microsoft.com/en-us/aspnet/core/migration/31-to-50?view=aspnetcore-3.1&tabs=visual-studio#datetime-values-are-model-bound-as-utc-times
        resultAsString.ShouldBe($"utc_{Kind.ToString().ToLower()}_{Kind.ToString().ToLower()}_utc");
    }

    [Fact]
    public async Task ComplexTypeDateTimeKind_JSON_Test()
    {
        var time = DateTime.Parse("2010-01-01T00:00:00Z");
        var response = await Client.PostAsync("/api/model-Binding-test/ComplexTypeDateTimeKind_JSON",
            new StringContent(JsonSerializer.Serialize(
                new GetDateTimeKindModel
                {
                    Time1 = time,
                    Time2 = time,
                    Time3 = time,
                    InnerModel = new GetDateTimeKindModel.GetDateTimeKindInnerModel
                    {
                        Time4 = time
                    }
                }
            ), Encoding.UTF8, MimeTypes.Application.Json));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var resultAsString = await response.Content.ReadAsStringAsync();
        resultAsString.ShouldBe($"local_{Kind.ToString().ToLower()}_{Kind.ToString().ToLower()}_local");
    }
}

public class ModelBindingController_Utc_Tests : ModelBindingController_Tests
{
    protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        Kind = DateTimeKind.Utc;
        services.Configure<AbpClockOptions>(x => x.Kind = Kind);

        base.ConfigureServices(context, services);
    }
}

public class ModelBindingController_Local_Tests : ModelBindingController_Tests
{
    protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        Kind = DateTimeKind.Local;
        services.Configure<AbpClockOptions>(x => x.Kind = Kind);

        base.ConfigureServices(context, services);
    }
}
