using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Shouldly;
using Xunit;

namespace Volo.Abp.Http.Client.Web;

public class AbpHttpClientProxyServiceConvention_Tests : AbpHttpClientWebTestBase
{
    [Fact]
    public async Task Should_Read_Primitive_Body_Parameter_From_Request_Body()
    {
        var response = await Client.PostAsync(
            "/api/client-proxy-test/from-body",
            new StringContent("\"Server=localhost;Database=Test\"", Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("Server=localhost;Database=Test");
    }

    [Fact]
    public async Task Should_Read_Non_String_Primitive_Body_Parameter_From_Request_Body()
    {
        var response = await Client.PostAsync(
            "/api/client-proxy-test/number-from-body",
            new StringContent("42", Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("42");
    }

    [Fact]
    public async Task Should_Keep_Reading_Other_Primitive_Parameters_From_Query_String()
    {
        var response = await Client.PostAsync("/api/client-proxy-test/from-query?value=test", null);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("test");
    }

    [Fact]
    public async Task Should_Read_Primitive_Parameters_From_Query_String_In_Get_Actions()
    {
        var guid = Guid.NewGuid();

        var response = await Client.GetAsync($"/api/client-proxy-test/query-parameters?number=42&flag=true&guid={guid}");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe($"42:True:{guid}");
    }

    [Fact]
    public async Task Should_Keep_Explicit_Binding_Attribute()
    {
        var response = await Client.PostAsync("/api/client-proxy-test/from-explicit-query?value=test", null);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("test");
    }

    [Fact]
    public async Task Should_Read_Complex_Parameter_From_Request_Body()
    {
        var response = await Client.PostAsync(
            "/api/client-proxy-test",
            new StringContent("{\"name\":\"test\",\"value\":42}", Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("test:42");
    }

    [Fact]
    public async Task Should_Read_Id_From_Route_And_Complex_Parameter_From_Request_Body()
    {
        var id = Guid.NewGuid();

        var response = await Client.PutAsync(
            $"/api/client-proxy-test/{id}",
            new StringContent("{\"name\":\"test\",\"value\":42}", Encoding.UTF8, "application/json"));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe($"{id}:test:42");
    }

    [Fact]
    public async Task Should_Read_Complex_Parameter_From_Query_String_In_Get_Actions()
    {
        var response = await Client.GetAsync("/api/client-proxy-test?Name=test&Value=42");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("test:42");
    }

    [Fact]
    public async Task Should_Read_Binding_From_Query_Types_From_Query_String()
    {
        var response = await Client.PostAsync("/api/client-proxy-test/by-query-type?Name=test&Value=42", null);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("test:42");
    }

    [Fact]
    public async Task Should_Read_Remote_Stream_Content_From_Form()
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent("file content"), "file", "test.txt");

        var response = await Client.PostAsync("/api/client-proxy-test/upload", content);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("test.txt:file content");
    }

    [Fact]
    public async Task Should_Use_Url_And_Http_Method_Of_Remote_Action()
    {
        var action = GetProxyAction<ClientProxyTestAppServiceClientProxy>(nameof(IClientProxyTestAppService.GetFromBodyAsync));

        action.AttributeRouteInfo.ShouldNotBeNull();
        action.AttributeRouteInfo.Template.ShouldBe("api/client-proxy-test/from-body");
        action.ActionConstraints.ShouldNotBeNull();
        action.ActionConstraints.OfType<HttpMethodActionConstraint>().Single().HttpMethods.ShouldBe(new[] { "POST" });

        var response = await Client.GetAsync("/api/client-proxy-test/from-body");
        response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public void Should_Use_Controller_Group_Name_As_Controller_Name()
    {
        var action = GetProxyAction<ClientProxyTestAppServiceClientProxy>(nameof(IClientProxyTestAppService.GetFromBodyAsync));

        action.ControllerName.ShouldBe("ClientProxyTest");
    }

    [Fact]
    public async Task Should_Remove_ClientProxy_Postfix_From_Controller_Name_Without_Controller_Group_Name()
    {
        var action = GetProxyAction<ClientProxyNoGroupNameAppServiceClientProxy>(nameof(IClientProxyNoGroupNameAppService.GetAsync));

        action.ControllerName.ShouldBe("ClientProxyNoGroupNameAppService");
        (await Client.GetStringAsync("/api/client-proxy-no-group-name")).ShouldBe("no-group-name");
    }

    [Fact]
    public void Should_Use_Root_Path_As_Area()
    {
        var action = GetProxyAction<ClientProxyTestAppServiceClientProxy>(nameof(IClientProxyTestAppService.GetFromBodyAsync));

        action.RouteValues["area"].ShouldBe("clientProxyTest");
    }

    [Fact]
    public void Should_Not_Expose_Action_Not_In_Api_Description()
    {
        GetActions()
            .Any(x => x.ControllerTypeInfo.AsType() == typeof(ClientProxyTestAppServiceClientProxy) &&
                      x.MethodInfo.Name == nameof(IClientProxyTestAppService.GetNotDescribedAsync))
            .ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Remove_Client_Proxy_Controller_If_Another_Controller_Implements_The_Same_Service()
    {
        GetActions()
            .Any(x => x.ControllerTypeInfo.AsType() == typeof(ClientProxyDuplicatedAppServiceClientProxy))
            .ShouldBeFalse();

        (await Client.GetStringAsync("/api/client-proxy-duplicated")).ShouldBe("controller");
        (await Client.GetAsync("/api/client-proxy-duplicated-proxy")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public void Should_Add_Client_Proxy_Actions_To_Api_Explorer()
    {
        var apiDescriptions = GetApiDescriptions("ClientProxyTest");

        apiDescriptions.ShouldContain(x => x.RelativePath == "api/client-proxy-test/from-body" && x.HttpMethod == "POST");
        apiDescriptions.ShouldContain(x => x.RelativePath == "api/client-proxy-test/{id}" && x.HttpMethod == "PUT");
    }

    [Fact]
    public async Task Should_Hide_Action_With_Disabled_Metadata_From_Api_Explorer()
    {
        GetApiDescriptions("ClientProxyTest").ShouldNotContain(x => x.RelativePath == "api/client-proxy-test/hidden");

        (await Client.GetStringAsync("/api/client-proxy-test/hidden")).ShouldBe("hidden");
    }

    private ControllerActionDescriptor[] GetActions()
    {
        return GetRequiredService<IActionDescriptorCollectionProvider>()
            .ActionDescriptors.Items
            .OfType<ControllerActionDescriptor>()
            .ToArray();
    }

    private ControllerActionDescriptor GetProxyAction<TClientProxy>(string methodName)
    {
        return GetActions().Single(x => x.ControllerTypeInfo.AsType() == typeof(TClientProxy) && x.MethodInfo.Name == methodName);
    }

    private ApiDescription[] GetApiDescriptions(string groupName)
    {
        return GetRequiredService<IApiDescriptionGroupCollectionProvider>()
            .ApiDescriptionGroups.Items
            .Where(x => x.GroupName == groupName)
            .SelectMany(x => x.Items)
            .ToArray();
    }
}
