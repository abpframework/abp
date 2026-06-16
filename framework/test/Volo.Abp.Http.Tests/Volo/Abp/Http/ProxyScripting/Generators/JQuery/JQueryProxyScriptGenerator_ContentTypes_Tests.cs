#nullable enable
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Xunit;

namespace Volo.Abp.Http.ProxyScripting.Generators.JQuery;

public class JQueryProxyScriptGenerator_ContentTypes_Tests
{
    private readonly JQueryProxyScriptGenerator _generator = new(
        Microsoft.Extensions.Options.Options.Create(new DynamicJavaScriptProxyOptions()));

    [Fact]
    public void Should_Emit_Json_DataType_And_Accept_When_ContentTypes_Contain_Json()
    {
        var script = _generator.CreateScript(BuildAppModel(
            returnType: "System.String",
            contentTypes: new[] { "text/plain", "application/json", "text/json" }));

        script.ShouldContain("dataType: 'json'");
        script.ShouldContain("Accept: 'application/json'");
        script.ShouldNotContain("dataType: 'text'");
    }

    [Fact]
    public void Should_Emit_Text_DataType_And_Accept_When_ContentTypes_Only_Text()
    {
        var script = _generator.CreateScript(BuildAppModel(
            returnType: "System.String",
            contentTypes: new[] { "text/plain", "text/csv" }));

        script.ShouldContain("dataType: 'text'");
        script.ShouldContain("Accept: 'text/plain'");
    }

    [Fact]
    public void Should_Fallback_To_Legacy_Text_When_Return_Is_String_And_ContentTypes_Null()
    {
        var script = _generator.CreateScript(BuildAppModel(
            returnType: "System.String",
            contentTypes: null));

        // Legacy behavior preserved: string return → dataType: 'text' (no Accept override)
        script.ShouldContain("dataType: 'text'");
        script.ShouldNotContain("Accept:");
    }

    [Fact]
    public void Should_Emit_No_DataType_For_Non_String_Return_Without_ContentTypes()
    {
        var script = _generator.CreateScript(BuildAppModel(
            returnType: "System.Int32",
            contentTypes: null));

        script.ShouldNotContain("dataType: 'text'");
        script.ShouldNotContain("dataType: 'json'");
    }

    [Fact]
    public void Should_Not_Emit_Json_DataType_When_Only_Binary_ContentTypes()
    {
        var script = _generator.CreateScript(BuildAppModel(
            returnType: "System.Byte[]",
            contentTypes: new[] { "application/octet-stream", "image/png" }));

        // jQuery dataType doesn't have a clean "blob" — fall through to no override
        script.ShouldNotContain("dataType: 'json'");
        script.ShouldNotContain("dataType: 'text'");
    }

    [Fact]
    public void Should_Prefer_Json_When_Json_Present_Even_With_Other_Types()
    {
        var script = _generator.CreateScript(BuildAppModel(
            returnType: "My.Project.UserDto",
            contentTypes: new[] { "application/xml", "application/json", "text/html" }));

        script.ShouldContain("dataType: 'json'");
        script.ShouldContain("Accept: 'application/json'");
    }

    [Fact]
    public void Case_Insensitive_Json_Detection()
    {
        var script = _generator.CreateScript(BuildAppModel(
            returnType: "System.String",
            contentTypes: new[] { "APPLICATION/JSON" }));

        script.ShouldContain("dataType: 'json'");
    }

    [Fact]
    public void IsRemoteStream_Should_Skip_DataType_Override_To_Avoid_JSON_Metadata_Regression()
    {
        // For IRemoteStreamContent returns the API definition still advertises
        // application/json (server-side default formatter list). The generator
        // MUST NOT force dataType:'json' + Accept:'application/json' for these —
        // doing so makes the server JSON-serialise the IRemoteStreamContent object
        // and re-introduces the original IRemoteStreamContent bug. jQuery doesn't
        // natively support binary downloads, so we let the legacy behavior stand.
        var model = BuildAppModel(
            returnType: "Volo.Abp.Content.IRemoteStreamContent",
            contentTypes: new[] { "text/plain", "application/json", "text/json" },
            isRemoteStream: true);

        var script = _generator.CreateScript(model);

        script.ShouldNotContain("dataType: 'json'");
        script.ShouldNotContain("Accept: 'application/json'");
    }

    private static ApplicationApiDescriptionModel BuildAppModel(string returnType, IList<string>? contentTypes, bool isRemoteStream = false)
    {
        var model = ApplicationApiDescriptionModel.Create();
        var module = model.GetOrAddModule("app", "Default");
        var controller = module.GetOrAddController(
            name: "TestController",
            groupName: null,
            isRemoteService: true,
            isIntegrationService: false,
            apiVersion: null,
            type: typeof(object));

        var action = new ActionApiDescriptionModel
        {
            UniqueName = "DoSomethingAsync",
            Name = "DoSomethingAsync",
            HttpMethod = "GET",
            Url = "api/test/do-something",
            SupportedVersions = new List<string>(),
            ParametersOnMethod = new List<MethodParameterApiDescriptionModel>(),
            Parameters = new List<ParameterApiDescriptionModel>(),
            ReturnValue = new ReturnValueApiDescriptionModel
            {
                Type = returnType,
                TypeSimple = returnType,
                ContentTypes = contentTypes,
                IsRemoteStream = isRemoteStream,
            },
            AuthorizeDatas = new List<AuthorizeDataApiDescriptionModel>(),
        };
        controller.AddAction("DoSomethingAsync", action);

        return model;
    }
}
