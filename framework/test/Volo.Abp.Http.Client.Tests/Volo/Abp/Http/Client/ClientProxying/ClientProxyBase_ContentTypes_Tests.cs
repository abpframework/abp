#nullable enable
using System.Collections.Generic;
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
    public void Mixed_Text_And_Octet_Stream_Should_Not_Pick_TextPlain()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "text/plain", "application/octet-stream" });

        InvokeGetAcceptForActionReturn(action).ShouldBeNull();
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
    public void Single_TextHtml_Should_Pick_TextPlain()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "text/html" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("text/plain");
    }

    [Fact]
    public void OctetStream_Only_With_ObjectReturn_Should_Return_Null()
    {
        var action = BuildAction(
            returnType: "My.Project.UserDto",
            contentTypes: new[] { "application/octet-stream" });

        InvokeGetAcceptForActionReturn(action).ShouldBeNull();
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
    public void Text_Json_Should_Be_Treated_As_Json()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "text/json" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/json");
    }

    [Fact]
    public void Application_Problem_Json_Should_Be_Treated_As_Json()
    {
        var action = BuildAction(
            returnType: "System.String",
            contentTypes: new[] { "application/problem+json" });

        InvokeGetAcceptForActionReturn(action).ShouldBe("application/json");
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

    private sealed class TestableClientProxy : ClientProxyBase<object>
    {
        public string? PublicGetAcceptForActionReturn(ActionApiDescriptionModel action)
            => GetAcceptForActionReturn(action);
    }
}
