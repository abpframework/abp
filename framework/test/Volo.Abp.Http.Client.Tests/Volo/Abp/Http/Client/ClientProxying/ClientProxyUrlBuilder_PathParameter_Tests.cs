#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Timing;
using Xunit;
using MicrosoftOptions = Microsoft.Extensions.Options.Options;

namespace Volo.Abp.Http.Client.ClientProxying;

public class ClientProxyUrlBuilder_PathParameter_Tests
{
    private readonly ClientProxyUrlBuilder _builder;
    private static readonly ApiVersionInfo PathApiVersion = new("Path", "1.0");

    public ClientProxyUrlBuilder_PathParameter_Tests()
    {
        _builder = CreateBuilder();
    }

    [Theory]
    [InlineData("plain", "api/files/plain")]
    [InlineData("a&b#c", "api/files/a%26b%23c")]
    [InlineData("a?b=c", "api/files/a%3Fb%3Dc")]
    [InlineData("a b", "api/files/a%20b")]
    [InlineData("a%23b", "api/files/a%2523b")]
    [InlineData("a/b#c", "api/files/a/b%23c")]
    [InlineData("rvA-a&b#<q>", "api/files/rvA-a%26b%23%3Cq%3E")]
    [InlineData("ä😀", "api/files/%C3%A4%F0%9F%98%80")]
    [InlineData("...", "api/files/...")]
    [InlineData("a..b", "api/files/a..b")]
    [InlineData(".hidden", "api/files/.hidden")]
    [InlineData("%2E%2E", "api/files/%252E%252E")]
    public async Task Should_Encode_Path_Value(string value, string expectedUrl)
    {
        var action = BuildAction(PathParam("name"));

        var url = await _builder.GenerateUrlWithParametersAsync(action, new Dictionary<string, object?> { ["name"] = value }, PathApiVersion);

        url.ShouldBe(expectedUrl);
    }

    [Theory]
    [InlineData(".")]
    [InlineData("..")]
    [InlineData("../admin")]
    [InlineData("a/../b")]
    [InlineData("a/./b")]
    [InlineData("a/..")]
    public async Task Should_Reject_Dot_Segments(string value)
    {
        var exception = await Should.ThrowAsync<AbpException>(() => GenerateUrlAsync(value));

        exception.Message.ShouldContain("name");
    }

    [Fact]
    public async Task Should_Reject_Dot_Segments_In_Default_Path_Value()
    {
        var action = BuildAction(PathParam("name", defaultValue: "../admin"));

        await Should.ThrowAsync<AbpException>(() => _builder.GenerateUrlWithParametersAsync(action, new Dictionary<string, object?>(), PathApiVersion));
    }

    [Fact]
    public async Task Should_Keep_Format_Of_Non_String_Path_Values()
    {
        var id = Guid.NewGuid();

        (await GenerateUrlAsync(42)).ShouldBe("api/files/42");
        (await GenerateUrlAsync(id)).ShouldBe($"api/files/{id}");
        (await GenerateUrlAsync(true)).ShouldBe("api/files/True");
        (await GenerateUrlAsync(DayOfWeek.Monday)).ShouldBe("api/files/Monday");
        (await GenerateUrlAsync(new DateTime(2026, 10, 8, 12, 30, 45, DateTimeKind.Utc))).ShouldBe("api/files/2026-10-08T12%3A30%3A45.0000000Z");
    }

    [Fact]
    public async Task Should_Keep_Empty_Path_Value_Empty()
    {
        (await GenerateUrlAsync(string.Empty)).ShouldBe("api/files/");
    }

    [Fact]
    public async Task Should_Encode_Path_And_Query_Values_Separately()
    {
        var action = BuildAction("api/files/{name}", PathParam("name"), QueryParam("search"));

        var url = await _builder.GenerateUrlWithParametersAsync(action, new Dictionary<string, object?> { ["name"] = "a b#c", ["search"] = "a b#c" }, PathApiVersion);

        url.ShouldBe("api/files/a%20b%23c?search=a+b%23c");
    }

    [Fact]
    public async Task Should_Replace_Path_Api_Version_And_Encode_Path_Value()
    {
        var action = BuildAction("api/v{apiVersion}/files/{name}", PathParam("apiVersion"), PathParam("name"));

        var url = await _builder.GenerateUrlWithParametersAsync(action, new Dictionary<string, object?> { ["name"] = "a#b" }, new ApiVersionInfo("Path", "1.0"));

        url.ShouldBe("api/v1.0/files/a%23b");
    }

    [Fact]
    public async Task Should_Add_Query_Api_Version_After_Encoded_Path_Value()
    {
        var action = BuildAction(PathParam("name"));

        var url = await _builder.GenerateUrlWithParametersAsync(action, new Dictionary<string, object?> { ["name"] = "a#b" }, new ApiVersionInfo("Query", "1.0"));

        url.ShouldBe("api/files/a%23b?api-version=1.0");
    }

    [Fact]
    public async Task Should_Not_Encode_Path_Converter_Result()
    {
        var builder = CreateBuilder(
            services => services.AddTransient<PreEncodedPathValueToPath>(),
            options => options.PathConverts.Add(typeof(PreEncodedPathValue), typeof(PreEncodedPathValueToPath)));
        var action = BuildAction(PathParam("name"));

        var url = await builder.GenerateUrlWithParametersAsync(action, new Dictionary<string, object?> { ["name"] = new PreEncodedPathValue("a%23b/c") }, PathApiVersion);

        url.ShouldBe("api/files/a%23b/c");
    }

    [Fact]
    public async Task Should_Encode_Default_Path_Value()
    {
        var action = BuildAction(PathParam("name", defaultValue: "a/b#c?d"));

        var url = await _builder.GenerateUrlWithParametersAsync(action, new Dictionary<string, object?>(), PathApiVersion);

        url.ShouldBe("api/files/a/b%23c%3Fd");
    }

    [Fact]
    public async Task Should_Remove_Missing_Optional_Path_Value()
    {
        var action = BuildAction(PathParam("name", isOptional: true, defaultValue: "a#b"));

        var url = await _builder.GenerateUrlWithParametersAsync(action, new Dictionary<string, object?>(), PathApiVersion);

        url.ShouldBe("api/files/");
    }

    private Task<string> GenerateUrlAsync(object value)
    {
        return _builder.GenerateUrlWithParametersAsync(BuildAction(PathParam("name")), new Dictionary<string, object?> { ["name"] = value }, PathApiVersion);
    }

    private static ClientProxyUrlBuilder CreateBuilder(Action<IServiceCollection>? configureServices = null, Action<AbpHttpClientProxyingOptions>? configureOptions = null)
    {
        var services = new ServiceCollection();
        configureServices?.Invoke(services);
        var proxyingOptions = new AbpHttpClientProxyingOptions();
        configureOptions?.Invoke(proxyingOptions);
        var scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
        return new ClientProxyUrlBuilder(scopeFactory, MicrosoftOptions.Create(proxyingOptions), new TestClock());
    }

    private static ActionApiDescriptionModel BuildAction(ParameterApiDescriptionModel parameter)
    {
        return BuildAction("api/files/{" + parameter.Name + "}", parameter);
    }

    private static ActionApiDescriptionModel BuildAction(string url, params ParameterApiDescriptionModel[] parameters)
    {
        return new ActionApiDescriptionModel
        {
            UniqueName = "Sample",
            Name = "Sample",
            HttpMethod = "GET",
            Url = url,
            SupportedVersions = new List<string>(),
            ParametersOnMethod = new List<MethodParameterApiDescriptionModel>(),
            Parameters = new List<ParameterApiDescriptionModel>(parameters)
        };
    }

    private static ParameterApiDescriptionModel PathParam(string name, bool isOptional = false, object? defaultValue = null)
    {
        return ParameterApiDescriptionModel.Create(
            name,
            jsonName: null,
            nameOnMethod: name,
            type: typeof(string),
            isOptional: isOptional,
            defaultValue: defaultValue,
            bindingSourceId: ParameterBindingSources.Path);
    }

    private static ParameterApiDescriptionModel QueryParam(string name)
    {
        return ParameterApiDescriptionModel.Create(
            name,
            jsonName: null,
            nameOnMethod: name,
            type: typeof(string),
            bindingSourceId: ParameterBindingSources.Query);
    }

    private record PreEncodedPathValue(string Value);

    private class PreEncodedPathValueToPath : IObjectToPath<PreEncodedPathValue>
    {
        public Task<string> ConvertAsync(ActionApiDescriptionModel actionApiDescription, ParameterApiDescriptionModel parameterApiDescription, PreEncodedPathValue value)
        {
            return Task.FromResult(value.Value);
        }
    }

    private class TestClock : IClock
    {
        public DateTime Now => DateTime.UtcNow;
        public DateTimeKind Kind => DateTimeKind.Utc;
        public bool SupportsMultipleTimezone => false;
        public DateTime Normalize(DateTime dateTime) => dateTime;
        public DateTime ConvertToUserTime(DateTime utcDateTime) => utcDateTime;
        public DateTimeOffset ConvertToUserTime(DateTimeOffset dateTimeOffset) => dateTimeOffset;
        public DateTime ConvertToUtc(DateTime dateTime) => dateTime;
    }
}
