using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Shouldly;
using Volo.Abp.AgentTools.Mcp.Sample;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.AgentTools.Mcp;

/// <summary>
/// Our endpoint and another module's in one app: neither serves anything of the other. Run for the other module's two forms
/// (attributes, own handlers), both registration orders and both transport modes of the other module.
/// </summary>
public abstract class OtherMcpModuleTests<TStartupModule> : McpIsolationTestBase
{
    private static readonly bool OtherHasOwnHandlers = typeof(TStartupModule).Name.StartsWith("OwnHandlers", StringComparison.Ordinal);

    protected override Type StartupModule => typeof(TStartupModule);

    [Fact]
    public void The_Other_Module_Is_Registered_In_The_Intended_Order()
    {
        var modules = GetRequiredService<IModuleContainer>().Modules.Select(m => m.Type).ToList();
        var other = modules.IndexOf(OtherHasOwnHandlers ? typeof(OtherOwnHandlersModule) : typeof(OtherAttributeToolsModule));

        other.ShouldBeGreaterThan(-1);
        (other < modules.IndexOf(typeof(AbpAgentToolsMcpModule))).ShouldBe(typeof(TStartupModule).Name.Contains("Before"));
    }

    public static TheoryData<string, McpCaller> Callers => new()
    {
        { OtherMcp.Path, McpCaller.Anonymous },
        { OtherMcp.Path, McpCaller.Cookie },
        { OtherMcp.Path, McpCaller.TokenWithoutScope },
        { OtherMcp.Path, McpCaller.TokenWithScope },
        { OtherMcp.SecurePath, McpCaller.Cookie },
        { OtherMcp.SecurePath, McpCaller.TokenWithoutScope },
        { OtherMcp.SecurePath, McpCaller.TokenWithScope }
    };

    [Theory]
    [MemberData(nameof(Callers))]
    public async Task The_Other_Endpoint_Does_Not_List_Our_Tools(string path, McpCaller caller)
    {
        var list = await RpcAsync(await CreateHttpClientAsync(caller), path, RequestMethods.ToolsList);

        list.Names("tools").ShouldBe(OtherMcp.ToolNames);
    }

    [Theory]
    [MemberData(nameof(Callers))]
    public async Task The_Other_Endpoint_Does_Not_Run_Our_Tools(string path, McpCaller caller)
    {
        await SeedNotesAsync(null, "our note");
        await SeedNotesAsync(TenantA, "our note");
        var client = await CreateHttpClientAsync(caller);

        foreach (var ours in new[] { McpSampleAppService.GetNotes, McpSampleAppService.CreateNote, McpSampleAppService.DeleteNote, McpSampleAppService.GetDisabled, "nope" })
        {
            var call = await CallAsync(client, path, ours, new { input = new { text = "from outside" }, id = Guid.NewGuid() });
            call.Body.ShouldNotContain("our note");
            call.Body.ShouldNotContain("AgentTools:");
            (call.ErrorCode != null || call.IsToolError).ShouldBeTrue(call.Body);
        }

        (await CallAsync(client, path, McpSampleAppService.GetSecret)).ToolText.ShouldBe("other: not the secret");
        // Neither our handlers nor the invoker were reached.
        Observer.Records.ShouldBeEmpty();
    }

    [Theory]
    [MemberData(nameof(Callers))]
    public async Task The_Other_Endpoint_Shows_Nothing_Of_Ours_In_Any_Other_Method(string path, McpCaller caller)
    {
        var client = await CreateHttpClientAsync(caller);

        var initialize = await InitializeAsync(client, path);
        initialize.Body.ShouldNotContain(OurServerName);
        initialize.Result.GetProperty("serverInfo").GetProperty("name").GetString().ShouldBe(OtherMcp.ServerName);
        initialize.Result.GetProperty("serverInfo").GetProperty("version").GetString().ShouldBe("9.9");
        foreach (var (method, parameters) in OtherMethods.Append((RequestMethods.LoggingSetLevel, new { level = "info" })))
        {
            var body = (await RpcAsync(client, path, method, parameters)).Body;
            body.ShouldNotContain("McpSample", customMessage: method);
            body.ShouldNotContain("AgentTools:", customMessage: method);
        }
    }

    [Theory]
    [InlineData(OtherMcp.Path, McpCaller.Anonymous)]
    [InlineData(OtherMcp.SecurePath, McpCaller.Cookie)]
    [InlineData(OtherMcp.SecurePath, McpCaller.TokenWithoutScope)]
    public async Task The_Other_Endpoint_Keeps_Serving_Its_Own(string path, McpCaller caller)
    {
        var client = await CreateHttpClientAsync(caller);

        var tools = await RpcAsync(client, path, RequestMethods.ToolsList);
        tools.Names("tools").ShouldBe(OtherMcp.ToolNames);
        (await CallAsync(client, path, OtherMcp.Echo, new { text = "hi" })).ToolText.ShouldBe("other: hi");
        (await CallAsync(client, path, McpSampleAppService.GetSecret)).ToolText.ShouldBe("other: not the secret");

        (await RpcAsync(client, path, RequestMethods.PromptsList)).Names("prompts").ShouldBe([OtherMcp.Prompt]);
        (await RpcAsync(client, path, RequestMethods.ResourcesList)).Names("resources").ShouldBe(["other_resource"]);
        (await RpcAsync(client, path, RequestMethods.ResourcesTemplatesList)).Names("resourceTemplates").ShouldBe(["other_item"]);
        foreach (var (method, parameters) in OtherMethods.Where(m => m.Method is RequestMethods.PromptsGet or RequestMethods.ResourcesRead or RequestMethods.CompletionComplete))
        {
            (await RpcAsync(client, path, method, parameters)).Body.ShouldContain("other: ", customMessage: method);
        }

        OtherLog.Entries.Distinct().ShouldBe(["filter message", "filter tools/list", "filter tools/call"], ignoreOrder: true);
    }

    [Fact]
    public async Task The_Other_Endpoint_Keeps_Its_Own_Authorization_And_Transport()
    {
        var anonymous = await PostInitializeAsync(CreateHttpClient(), OtherMcp.SecurePath);
        anonymous.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        // Stateful: sessions, and GET and DELETE are mapped (and ask for one). Stateless: none of them.
        (await PostInitializeAsync(CreateHttpClient(), OtherMcp.Path)).Headers.Contains("Mcp-Session-Id").ShouldBe(!OtherIsStateless);
        var get = new HttpRequestMessage(HttpMethod.Get, OtherMcp.Path);
        get.Headers.Accept.ParseAdd("text/event-stream");
        (await CreateHttpClient().SendAsync(get)).StatusCode.ShouldBe(OtherIsStateless ? HttpStatusCode.MethodNotAllowed : HttpStatusCode.BadRequest);
        (await CreateHttpClient().DeleteAsync(OtherMcp.Path)).StatusCode.ShouldBe(OtherIsStateless ? HttpStatusCode.MethodNotAllowed : HttpStatusCode.OK);
    }

    [Fact]
    public async Task Our_Endpoint_Lists_And_Runs_Only_Our_Tools()
    {
        await SeedNotesAsync(TenantA, "our note");
        var client = CreateHttpClient(PermittedUser());

        (await CallAsync(client, OurPath, OtherMcp.Echo, new { text = "hi" })).ToolText
            .ShouldBe($"ERROR BusinessException: There is no agent tool named '{OtherMcp.Echo}'. (code: {AgentToolsErrorCodes.ToolNotFound})");
        (await CallAsync(client, OurPath, McpSampleAppService.GetSecret)).ToolText.ShouldBe("\"the secret\"");
        (await CallAsync(client, OurPath, McpSampleAppService.GetNotes)).ToolText.ShouldBe("[\"our note\"]");

        var tools = await RpcAsync(client, OurPath, RequestMethods.ToolsList);
        tools.Names("tools").ShouldBe(OurTools);
        tools.Body.ShouldNotContain("other");
        tools.HasSession.ShouldBeFalse();
        OtherLog.Entries.ShouldBeEmpty();
    }

    [Fact]
    public async Task Our_Endpoint_Answers_No_Other_Method_Of_The_Other_Module()
    {
        var client = CreateHttpClient(PermittedUser());

        foreach (var (method, parameters) in OtherMethods)
        {
            var response = await RpcAsync(client, OurPath, method, parameters);
            response.ErrorCode.ShouldBe(-32601, $"{method}: {response.Body}");
        }

        (await RpcAsync(client, OurPath, RequestMethods.LoggingSetLevel, new { level = "info" })).ErrorCode.ShouldBeNull();
        OtherLog.Entries.ShouldBeEmpty();
    }

    [Fact]
    public async Task Our_Endpoint_Introduces_Itself()
    {
        var initialize = await InitializeAsync(CreateHttpClient(PermittedUser()), OurPath);

        initialize.Result.GetProperty("serverInfo").GetProperty("name").GetString().ShouldBe(OurServerName);
        initialize.Result.GetProperty("capabilities").EnumerateObject().Select(p => p.Name).Order().ShouldBe(["logging", "tools"]);
        OtherLog.Entries.ShouldBeEmpty();
    }

    [Fact]
    public void Nothing_Of_Ours_Is_In_The_App_Wide_Server_Options()
    {
        foreach (var options in new[]
                 {
                     GetRequiredService<IOptions<McpServerOptions>>().Value,
                     GetRequiredService<IOptionsFactory<McpServerOptions>>().Create(Microsoft.Extensions.Options.Options.DefaultName)
                 })
        {
            options.ServerInfo!.Name.ShouldBe(OtherMcp.ServerName);
            (options.ToolCollection?.PrimitiveNames ?? []).ShouldBe(OtherHasOwnHandlers ? [] : OtherMcp.ToolNames, ignoreOrder: true);
            options.Handlers.ListToolsHandler?.Method.DeclaringType!.Assembly.ShouldBe(typeof(OtherTools).Assembly);
            options.Handlers.CallToolHandler?.Method.DeclaringType!.Assembly.ShouldBe(typeof(OtherTools).Assembly);
            (options.Handlers.ListToolsHandler != null).ShouldBe(OtherHasOwnHandlers);
            (options.Handlers.CallToolHandler != null).ShouldBe(OtherHasOwnHandlers);
        }
    }
}

public class AttributeToolsBeforeTests : OtherMcpModuleTests<AttributeToolsBeforeModule>;

public class AttributeToolsAfterTests : OtherMcpModuleTests<AttributeToolsAfterModule>;

public class OwnHandlersBeforeTests : OtherMcpModuleTests<OwnHandlersBeforeModule>;

public class OwnHandlersAfterTests : OtherMcpModuleTests<OwnHandlersAfterModule>;

public class StatelessAttributeToolsBeforeTests : AttributeToolsBeforeTests
{
    protected override bool OtherIsStateless => true;
}

public class StatelessAttributeToolsAfterTests : AttributeToolsAfterTests
{
    protected override bool OtherIsStateless => true;
}

public class StatelessOwnHandlersBeforeTests : OwnHandlersBeforeTests
{
    protected override bool OtherIsStateless => true;
}

public class StatelessOwnHandlersAfterTests : OwnHandlersAfterTests
{
    protected override bool OtherIsStateless => true;
}

/// <summary>[Authorize] tools of the other module, without the SDK's authorization filters (its own endpoint then fails closed).</summary>
public class AuthorizedToolsWithoutFiltersBeforeTests : McpIsolationTestBase
{
    protected override Type StartupModule => typeof(AttributeToolsBeforeModule);

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddMcpServer().WithTools<OtherAuthorizedTools>();
    }

    [Fact]
    public async Task Our_Tools_Are_Listed_And_Callable()
    {
        var client = CreateHttpClient(PermittedUser());

        (await RpcAsync(client, OurPath, RequestMethods.ToolsList)).Names("tools").ShouldBe(OurTools);
        (await CallAsync(client, OurPath, McpSampleAppService.GetNotes)).ToolText.ShouldBe("[]");
        (await CallAsync(client, OurPath, OtherAuthorizedTools.Name)).ToolText.ShouldContain(AgentToolsErrorCodes.ToolNotFound);
    }
}

public class AuthorizedToolsWithoutFiltersAfterTests : AuthorizedToolsWithoutFiltersBeforeTests
{
    protected override Type StartupModule => typeof(AttributeToolsAfterModule);
}

public class AuthorizedToolsWithFiltersBeforeTests : AuthorizedToolsWithoutFiltersBeforeTests
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddMcpServer().AddAuthorizationFilters();
    }

    [Fact]
    public async Task The_Other_Endpoint_Serves_Its_Authorized_Tool_To_A_Signed_In_User_Only()
    {
        var anonymous = CreateHttpClient();
        (await RpcAsync(anonymous, OtherMcp.Path, RequestMethods.ToolsList)).Names("tools").ShouldBe(OtherMcp.ToolNames);
        (await CallAsync(anonymous, OtherMcp.Path, OtherAuthorizedTools.Name)).ErrorCode.ShouldNotBeNull();

        var user = await CreateHttpClientAsync(McpCaller.Cookie);
        (await RpcAsync(user, OtherMcp.SecurePath, RequestMethods.ToolsList)).Names("tools")
            .ShouldBe([McpSampleAppService.GetSecret, OtherAuthorizedTools.Name, OtherMcp.Echo]);
        (await CallAsync(user, OtherMcp.SecurePath, OtherAuthorizedTools.Name)).ToolText.ShouldBe("other: authorized");
    }
}

public class AuthorizedToolsWithFiltersAfterTests : AuthorizedToolsWithFiltersBeforeTests
{
    protected override Type StartupModule => typeof(AttributeToolsAfterModule);
}
