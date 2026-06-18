#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Shouldly;
using Volo.Abp.Content;
using Volo.Abp.Http.Modeling;
using Xunit;

namespace Volo.Abp.Http.Client.ClientProxying;

public class ClientProxyBase_GetAcceptForActionReturn_Tests
{
    [Fact]
    public void IRemoteStreamContent_Should_Pick_OctetStream_Even_When_ContentTypes_Include_Json()
    {
        var action = BuildAction(
            returnType: typeof(IRemoteStreamContent).FullName!,
            contentTypes: new[] { "application/json", "text/plain", "text/json" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/octet-stream");
    }

    [Fact]
    public void RemoteStreamContent_Concrete_Type_Should_Pick_OctetStream()
    {
        var action = BuildAction(
            returnType: typeof(RemoteStreamContent).FullName!,
            contentTypes: null);

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/octet-stream");
    }

    [Fact]
    public void Json_In_ContentTypes_Should_Pick_Json()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "text/plain", "application/json", "text/json" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/json");
    }

    [Fact]
    public void Only_Text_ContentTypes_Should_Pick_TextPlain()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "text/plain", "text/csv" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("text/plain");
    }

    [Fact]
    public void Empty_Or_Null_ContentTypes_Should_Return_Null()
    {
        InvokeGetAcceptForActionReturn(BuildAction("System.Int32", null)).ShouldBeNull();
        InvokeGetAcceptForActionReturn(BuildAction("System.Int32", new string[0])).ShouldBeNull();
    }

    [Fact]
    public void Mixed_Text_And_Octet_Stream_Should_Echo_First_Content_Type()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "text/plain", "application/octet-stream" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("text/plain");
    }

    [Fact]
    public void JsonV2_Variant_Should_Still_Pick_Json()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "application/json; charset=utf-8" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/json");
    }

    [Fact]
    public void Single_TextHtml_Should_Echo_Back_TextHtml()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "text/html" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("text/html");
    }

    [Fact]
    public void OctetStream_Only_With_ObjectReturn_Should_Echo_OctetStream()
    {
        var action = BuildAction(
            returnType: "My.Project.UserDto",
            contentTypes: new[] { "application/octet-stream" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/octet-stream");
    }

    [Fact]
    public void ApplicationXml_Only_Should_Echo_Back_Xml_Instead_Of_Legacy_Pair()
    {
        var action = BuildAction(
            returnType: "My.Project.SoapEnvelope",
            contentTypes: new[] { "application/xml" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/xml");
    }

    [Fact]
    public void Case_Insensitive_Json_Match()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "APPLICATION/JSON" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/json");
    }

    [Fact]
    public void Json_With_Charset_Parameter_Should_Still_Pick_Json()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "application/json; charset=utf-8" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/json");
    }

    [Fact]
    public void Text_With_Charset_Parameter_Should_Still_Pick_TextPlain()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "text/plain ; charset=utf-8 " });

        InvokeGetAcceptForActionReturn(action).ShouldBe("text/plain");
    }

    [Fact]
    public void Text_Json_Should_Echo_Back_Text_Json()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "text/json" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("text/json");
    }

    [Fact]
    public void Application_Problem_Json_Should_Echo_Back_The_Plus_Json_Variant()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "application/problem+json" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/problem+json");
    }

    [Fact]
    public void Vendor_Plus_Json_Should_Echo_Back_The_Plus_Json_Variant()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "application/vnd.api+json" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/vnd.api+json");
    }

    [Fact]
    public void IsRemoteStream_Flag_True_Should_Pick_OctetStream_Regardless_Of_TypeName()
    {
        var action = BuildAction(
            returnType: "My.Project.CustomStream",
            contentTypes: new[] { "application/json" });
        action.ReturnValue.IsRemoteStream = true;

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/octet-stream");
    }

    private static string? InvokeGetAcceptForActionReturn(ActionApiDescriptionModel action)
    {
        var proxy = new TestableClientProxy();
        return proxy.PublicGetAcceptForActionReturn(action);
    }

    private static ActionApiDescriptionModel BuildAction(string returnType, IList<string>? contentTypes)
    {
        return new ActionApiDescriptionModel
        {
            UniqueName = "Sample",
            Name = "Sample",
            HttpMethod = "GET",
            Url = "api/test",
            SupportedVersions = new List<string>(),
            ParametersOnMethod = new List<MethodParameterApiDescriptionModel>(),
            Parameters = new List<ParameterApiDescriptionModel>(),
            ReturnValue = new ReturnValueApiDescriptionModel
            {
                Type = returnType,
                TypeSimple = returnType,
                ContentTypes = contentTypes
            },
            AuthorizeDatas = new List<AuthorizeDataApiDescriptionModel>()
        };
    }

    [Fact]
    public void AddHeaders_With_ApiVersion_Should_Combine_OctetStream_Accept_With_Version_Suffix()
    {
        var action = BuildAction(
            returnType: typeof(IRemoteStreamContent).FullName!,
            contentTypes: new[] { "application/json", "text/plain" });

        var headers = InvokeAddHeadersAndCollectAccept(action, version: "2.0");

        headers.ShouldContain("application/octet-stream; v=2.0");
        headers.ShouldNotContain(h => h == "text/plain; v=2.0");
        headers.ShouldNotContain(h => h == "application/json; v=2.0");
    }

    [Fact]
    public void AddHeaders_Without_ApiVersion_Should_Emit_OctetStream_For_Stream_Returns()
    {
        var action = BuildAction(
            returnType: typeof(IRemoteStreamContent).FullName!,
            contentTypes: new[] { "application/json" });

        var headers = InvokeAddHeadersAndCollectAccept(action, version: null);

        headers.ShouldContain("application/octet-stream");
    }

    [Fact]
    public void AddHeaders_Without_ContentType_Metadata_Should_Fall_Back_To_Text_And_Json_Pair()
    {
        var action = BuildAction(returnType: "System.Int32", contentTypes: null);

        var headers = InvokeAddHeadersAndCollectAccept(action, version: "1.0");

        headers.ShouldContain("text/plain; v=1.0");
        headers.ShouldContain("application/json; v=1.0");
    }

    [Fact]
    public void AddHeaders_Without_ApiVersion_And_Without_ContentType_Metadata_Should_Emit_Unversioned_Text_Json_Pair()
    {
        var action = BuildAction(returnType: "System.Int32", contentTypes: null);

        var headers = InvokeAddHeadersAndCollectAccept(action, version: null);

        headers.ShouldContain("text/plain");
        headers.ShouldContain("application/json");
        headers.ShouldNotContain(h => h.Contains("; v="));
    }

    private static IList<string> InvokeAddHeadersAndCollectAccept(ActionApiDescriptionModel action, string? version)
    {
        var proxy = new TestableClientProxy();
        var message = new HttpRequestMessage(HttpMethod.Get, "http://localhost/x");
        var apiVersion = new ApiVersionInfo("HeaderModelBinding", version ?? string.Empty);
        proxy.PublicAddAcceptHeaders(action, message, apiVersion);
        return message.Headers.Accept.Select(a => a.ToString()).ToList();
    }

    private sealed class TestableClientProxy : ClientProxyBase<object>
    {
        public string? PublicGetAcceptForActionReturn(ActionApiDescriptionModel action)
            => GetAcceptForActionReturn(action);

        public void PublicAddAcceptHeaders(ActionApiDescriptionModel action, HttpRequestMessage requestMessage, ApiVersionInfo apiVersion)
            => AddAcceptHeaders(action, requestMessage, apiVersion);
    }
}
