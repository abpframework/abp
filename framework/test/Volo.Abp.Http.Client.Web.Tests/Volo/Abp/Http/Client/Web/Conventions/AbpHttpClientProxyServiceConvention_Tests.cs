using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Shouldly;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Abp.Http.Client.ClientProxying;
using Xunit;

namespace Volo.Abp.Http.Client.Web.Conventions;

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

    [Theory]
    [InlineData(nameof(IClientProxyTestAppService.GetNotDescribedAsync))]
    [InlineData(nameof(IClientProxyTestAppService.GetNotDescribedWithRemoteServiceAsync))]
    [InlineData(nameof(IClientProxyTestAppService.GetNotDescribedWithVerbAsync))]
    [InlineData(nameof(IClientProxyTestAppService.GetNotDescribedHiddenAsync))]
    [InlineData(nameof(IClientProxyTestAppService.GetNotDescribedIgnoredAsync))]
    [InlineData(nameof(IClientProxyTestAppService.GetDisabledAsync))]
    [InlineData(nameof(IClientProxyTestAppService.GetDisabledWithVerbAsync))]
    public async Task Should_Not_Expose_Action_Without_Route(string methodName)
    {
        FindProxyAction<ClientProxyTestAppServiceClientProxy>(methodName).ShouldBeNull();

        var response = await Client.GetAsync($"/clientProxyTest/ClientProxyTest/{methodName.RemovePostFix("Async")}");
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Not_Expose_Action_Without_Route_When_Controller_Has_No_Other_Route()
    {
        FindProxyAction<ClientProxyDisabledActionAppServiceClientProxy>(nameof(IClientProxyDisabledActionAppService.GetAsync)).ShouldBeNull();

        (await Client.GetAsync("/clientProxyTest/ClientProxyDisabledAction/Get")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await Client.GetAsync("/api/client-proxy-disabled-action")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Not_Expose_Actions_Of_Disabled_Client_Proxy_Without_Explicit_Route()
    {
        FindProxyAction<ClientProxyDisabledAppServiceClientProxy>(nameof(IClientProxyDisabledAppService.GetAsync)).ShouldBeNull();

        (await Client.GetAsync("/clientProxyTest/ClientProxyDisabled/Get")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await Client.GetAsync("/api/client-proxy-disabled")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        (await Client.GetStringAsync("/api/client-proxy-disabled/with-route")).ShouldBe("disabled-with-route");
    }

    [Theory]
    [InlineData(nameof(IClientProxyTestAppService.GetDisabledWithRouteAsync), "api/client-proxy-test/disabled-with-route", false)]
    [InlineData(nameof(IClientProxyTestAppService.GetNotDescribedWithRouteAsync), "api/client-proxy-test/not-described-with-route", true)]
    public async Task Should_Keep_Explicit_Action_Route(string methodName, string url, bool visibleInApiExplorer)
    {
        GetProxyAction<ClientProxyTestAppServiceClientProxy>(methodName).AttributeRouteInfo.Template.ShouldBe(url);

        (await Client.GetAsync($"/{url}")).StatusCode.ShouldBe(HttpStatusCode.OK);
        GetApiDescriptions("ClientProxyTest").Any(x => x.RelativePath == url).ShouldBe(visibleInApiExplorer);
    }

    [Fact]
    public async Task Should_Keep_Only_Explicit_Route_Of_Action_With_Multiple_Selectors()
    {
        var action = GetProxyAction<ClientProxyTestAppServiceClientProxy>(nameof(IClientProxyTestAppService.GetDisabledWithVerbAndRouteAsync));
        action.AttributeRouteInfo.Template.ShouldBe("api/client-proxy-test/disabled-with-verb-and-route");

        (await Client.PostAsync("/api/client-proxy-test/disabled-with-verb-and-route", null)).StatusCode.ShouldBe(HttpStatusCode.OK);
        (await Client.GetAsync("/clientProxyTest/ClientProxyTest/GetDisabledWithVerbAndRoute")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Keep_Explicit_Controller_Route()
    {
        (await Client.GetStringAsync("/api/client-proxy-controller-route")).ShouldBe("controller-route");
        (await Client.GetAsync("/api/client-proxy-controller-route-json")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Should_Hide_Client_Proxy_With_Disabled_Metadata_From_Api_Explorer()
    {
        (await Client.GetStringAsync("/api/client-proxy-metadata-disabled")).ShouldBe("metadata-disabled");
        GetApiDescriptions("ClientProxyMetadataDisabled").ShouldBeEmpty();
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

    private ControllerActionDescriptor FindProxyAction<TClientProxy>(string methodName)
    {
        return GetActions().SingleOrDefault(x => x.ControllerTypeInfo.AsType() == typeof(TClientProxy) && x.MethodInfo.Name == methodName);
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

public interface IClientProxyTestAppService : IApplicationService
{
    Task<string> GetFromBodyAsync(string value);

    Task<string> GetFromQueryAsync(string value);

    Task<int> GetNumberFromBodyAsync(int value);

    Task<string> GetFromExplicitQueryAsync(string value);

    Task<string> GetQueryParametersAsync(int number, bool flag, Guid guid);

    Task<string> CreateAsync(ClientProxyTestInput input);

    Task<string> UpdateAsync(Guid id, ClientProxyTestInput input);

    Task<string> GetListAsync(ClientProxyTestInput input);

    Task<string> GetByQueryTypeAsync(ClientProxyTestQueryInput input);

    Task<string> UploadAsync(IRemoteStreamContent file);

    Task<string> GetHiddenAsync();

    Task<string> GetNotDescribedAsync();

    Task<string> GetDisabledAsync();

    Task<string> GetDisabledWithVerbAsync();

    Task<string> GetNotDescribedWithRemoteServiceAsync();

    Task<string> GetNotDescribedWithVerbAsync();

    Task<string> GetNotDescribedHiddenAsync();

    Task<string> GetNotDescribedIgnoredAsync();

    Task<string> GetDisabledWithRouteAsync();

    Task<string> GetNotDescribedWithRouteAsync();

    Task<string> GetDisabledWithVerbAndRouteAsync();
}

// Returns the received values instead of calling a remote service, so the tests can see what the proxy controller bound.
public class ClientProxyTestAppServiceClientProxy : ClientProxyBase<IClientProxyTestAppService>, IClientProxyTestAppService
{
    public virtual Task<string> GetFromBodyAsync(string value)
    {
        return Task.FromResult(value);
    }

    public virtual Task<string> GetFromQueryAsync(string value)
    {
        return Task.FromResult(value);
    }

    public virtual Task<int> GetNumberFromBodyAsync(int value)
    {
        return Task.FromResult(value);
    }

    public virtual Task<string> GetFromExplicitQueryAsync([FromQuery] string value)
    {
        return Task.FromResult(value);
    }

    public virtual Task<string> GetQueryParametersAsync(int number, bool flag, Guid guid)
    {
        return Task.FromResult($"{number}:{flag}:{guid}");
    }

    public virtual Task<string> CreateAsync(ClientProxyTestInput input)
    {
        return Task.FromResult($"{input.Name}:{input.Value}");
    }

    public virtual Task<string> UpdateAsync(Guid id, ClientProxyTestInput input)
    {
        return Task.FromResult($"{id}:{input.Name}:{input.Value}");
    }

    public virtual Task<string> GetListAsync(ClientProxyTestInput input)
    {
        return Task.FromResult($"{input.Name}:{input.Value}");
    }

    public virtual Task<string> GetByQueryTypeAsync(ClientProxyTestQueryInput input)
    {
        return Task.FromResult($"{input.Name}:{input.Value}");
    }

    public virtual async Task<string> UploadAsync(IRemoteStreamContent file)
    {
        using var reader = new StreamReader(file.GetStream());
        return $"{file.FileName}:{await reader.ReadToEndAsync()}";
    }

    [RemoteService(IsMetadataEnabled = false)]
    public virtual Task<string> GetHiddenAsync()
    {
        return Task.FromResult("hidden");
    }

    public virtual Task<string> GetNotDescribedAsync()
    {
        return Task.FromResult("not-described");
    }

    [RemoteService(false)]
    public virtual Task<string> GetDisabledAsync()
    {
        return Task.FromResult("disabled");
    }

    [RemoteService(false)]
    [HttpGet]
    public virtual Task<string> GetDisabledWithVerbAsync()
    {
        return Task.FromResult("disabled-with-verb");
    }

    [RemoteService]
    public virtual Task<string> GetNotDescribedWithRemoteServiceAsync()
    {
        return Task.FromResult("not-described-with-remote-service");
    }

    [HttpGet]
    public virtual Task<string> GetNotDescribedWithVerbAsync()
    {
        return Task.FromResult("not-described-with-verb");
    }

    [RemoteService(IsMetadataEnabled = false)]
    public virtual Task<string> GetNotDescribedHiddenAsync()
    {
        return Task.FromResult("not-described-hidden");
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public virtual Task<string> GetNotDescribedIgnoredAsync()
    {
        return Task.FromResult("not-described-ignored");
    }

    [RemoteService(false)]
    [HttpGet("api/client-proxy-test/disabled-with-route")]
    public virtual Task<string> GetDisabledWithRouteAsync()
    {
        return Task.FromResult("disabled-with-route");
    }

    [HttpGet("api/client-proxy-test/not-described-with-route")]
    public virtual Task<string> GetNotDescribedWithRouteAsync()
    {
        return Task.FromResult("not-described-with-route");
    }

    [RemoteService(false)]
    [HttpGet]
    [HttpPost("api/client-proxy-test/disabled-with-verb-and-route")]
    public virtual Task<string> GetDisabledWithVerbAndRouteAsync()
    {
        return Task.FromResult("disabled-with-verb-and-route");
    }
}

public class ClientProxyTestInput
{
    public string Name { get; set; }

    public int Value { get; set; }
}

public class ClientProxyTestQueryInput
{
    public string Name { get; set; }

    public int Value { get; set; }
}

public interface IClientProxyNoGroupNameAppService : IApplicationService
{
    Task<string> GetAsync();
}

public class ClientProxyNoGroupNameAppServiceClientProxy : ClientProxyBase<IClientProxyNoGroupNameAppService>, IClientProxyNoGroupNameAppService
{
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("no-group-name");
    }
}

public interface IClientProxyDuplicatedAppService : IApplicationService
{
    Task<string> GetAsync();
}

public class ClientProxyDuplicatedAppServiceClientProxy : ClientProxyBase<IClientProxyDuplicatedAppService>, IClientProxyDuplicatedAppService
{
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("proxy");
    }
}

[Route("api/client-proxy-duplicated")]
public class ClientProxyDuplicatedController : AbpControllerBase, IClientProxyDuplicatedAppService
{
    [HttpGet]
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("controller");
    }
}

public interface IClientProxyDisabledAppService : IApplicationService
{
    Task<string> GetAsync();

    Task<string> GetWithRouteAsync();
}

[RemoteService(false)]
public class ClientProxyDisabledAppServiceClientProxy : ClientProxyBase<IClientProxyDisabledAppService>, IClientProxyDisabledAppService
{
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("disabled");
    }

    [HttpGet("api/client-proxy-disabled/with-route")]
    public virtual Task<string> GetWithRouteAsync()
    {
        return Task.FromResult("disabled-with-route");
    }
}

public interface IClientProxyDisabledActionAppService : IApplicationService
{
    Task<string> GetAsync();
}

public class ClientProxyDisabledActionAppServiceClientProxy : ClientProxyBase<IClientProxyDisabledActionAppService>, IClientProxyDisabledActionAppService
{
    [RemoteService(false)]
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("disabled-action");
    }
}

public interface IClientProxyMetadataDisabledAppService : IApplicationService
{
    Task<string> GetAsync();
}

[RemoteService(IsMetadataEnabled = false)]
public class ClientProxyMetadataDisabledAppServiceClientProxy : ClientProxyBase<IClientProxyMetadataDisabledAppService>, IClientProxyMetadataDisabledAppService
{
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("metadata-disabled");
    }
}

public interface IClientProxyControllerRouteAppService : IApplicationService
{
    Task<string> GetAsync();
}

[Route("api/client-proxy-controller-route")]
public class ClientProxyControllerRouteAppServiceClientProxy : ClientProxyBase<IClientProxyControllerRouteAppService>, IClientProxyControllerRouteAppService
{
    [RemoteService(false)]
    [HttpGet]
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("controller-route");
    }
}
