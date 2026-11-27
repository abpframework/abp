using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AgentTools.Mcp.Sample;
using Xunit;

namespace Volo.Abp.AgentTools.Mcp;

public class McpToolTests : AgentToolsMcpTestBase
{
    [Fact]
    public async Task Permitted_User_Gets_Exactly_The_Enabled_Non_Approval_Permitted_Tools_With_Annotations_And_Descriptions()
    {
        await using var client = await ConnectAsync(NewUser(TenantA, McpSamplePermissions.Secret));

        var tools = (await client.ListToolsAsync()).Select(t => t.ProtocolTool).ToList();

        tools.Select(t => t.Name).OrderBy(n => n).ShouldBe([McpSampleAppService.CreateNote, McpSampleAppService.GetNotes, McpSampleAppService.GetSecret]);
        var catalog = GetRequiredService<IAgentToolCatalog>();
        foreach (var tool in tools)
        {
            var definition = catalog.Find(tool.Name)!;
            tool.Description.ShouldBe(definition.Description);
            tool.Annotations!.ReadOnlyHint.ShouldBe(definition.ReadOnly);
            tool.Annotations.DestructiveHint.ShouldBe(definition.Destructive);
            tool.Annotations.IdempotentHint.ShouldBe(definition.Idempotent);
        }

        var notes = tools.Single(t => t.Name == McpSampleAppService.GetNotes);
        notes.Description.ShouldBe("Lists the notes of the current tenant.");
        notes.Annotations!.ReadOnlyHint.ShouldBe(true);
        var create = tools.Single(t => t.Name == McpSampleAppService.CreateNote);
        create.Annotations!.ReadOnlyHint.ShouldBe(false);
        create.InputSchema.GetProperty("properties").GetProperty("input").ValueKind.ShouldBe(JsonValueKind.Object);
        create.InputSchema.GetProperty("required").EnumerateArray().Select(e => e.GetString()).ShouldBe(["input"]);
    }

    [Fact]
    public async Task Less_Privileged_User_Gets_Fewer_Tools()
    {
        await using var client = await ConnectAsync(NewUser(TenantA));

        (await client.ListToolsAsync()).Select(t => t.Name).OrderBy(n => n)
            .ShouldBe([McpSampleAppService.CreateNote, McpSampleAppService.GetNotes]);
    }

    [Fact]
    public async Task Approval_Required_Disabled_And_Unknown_Tools_Are_Refused_By_Name()
    {
        await using var client = await ConnectAsync(NewUser(TenantA, McpSamplePermissions.Secret));

        var delete = await CallAsync(client, McpSampleAppService.DeleteNote, new { id = Guid.NewGuid() });
        delete.IsError.ShouldBe(true);
        Text(delete).ShouldBe($"ERROR BusinessException: The agent tool '{McpSampleAppService.DeleteNote}' requires human approval, " +
                              $"which this client does not support. (code: {AgentToolsErrorCodes.ApprovalNotSupported})");

        var disabled = await CallAsync(client, McpSampleAppService.GetDisabled);
        disabled.IsError.ShouldBe(true);
        Text(disabled).ShouldBe($"ERROR BusinessException: The agent tool '{McpSampleAppService.GetDisabled}' is disabled. (code: {AgentToolsErrorCodes.ToolDisabled})");

        var unknown = await CallAsync(client, "McpSample_Nope");
        unknown.IsError.ShouldBe(true);
        Text(unknown).ShouldBe($"ERROR BusinessException: There is no agent tool named 'McpSample_Nope'. (code: {AgentToolsErrorCodes.ToolNotFound})");
    }

    [Fact]
    public async Task Tool_The_User_May_Not_Call_Is_Absent_And_Refused_By_Name_With_The_Authorization_Error()
    {
        await using var client = await ConnectAsync(NewUser(TenantA));

        var result = await CallAsync(client, McpSampleAppService.GetSecret);

        result.IsError.ShouldBe(true);
        Text(result).ShouldStartWith("ERROR AbpAuthorizationException: ");
    }

    [Fact]
    public async Task Success_Returns_The_Json_Result_And_Errors_Are_Tool_Errors_On_A_Connection_That_Keeps_Working()
    {
        await using var client = await ConnectAsync(NewUser(TenantA));

        var created = await CallAsync(client, McpSampleAppService.CreateNote, new { input = new { text = "hello" } });
        created.IsError.ShouldNotBe(true);
        JsonSerializer.Deserialize<JsonElement>(Text(created)).GetProperty("text").GetString().ShouldBe("hello");

        var invalid = await CallAsync(client, McpSampleAppService.CreateNote, new { input = new { } });
        invalid.IsError.ShouldBe(true);
        Text(invalid).ShouldStartWith("ERROR AbpValidationException: ");
        Text(invalid).ShouldContain("[Text]");

        var business = await CallAsync(client, McpSampleAppService.CreateNote, new { input = new { text = "fail" } });
        business.IsError.ShouldBe(true);
        Text(business).ShouldBe("ERROR BusinessException: A note cannot say fail. (code: McpSample:NoFail)");

        var notes = await CallAsync(client, McpSampleAppService.GetNotes);
        notes.IsError.ShouldNotBe(true);
        Text(notes).ShouldBe("[\"hello\"]");
    }

    [Fact]
    public async Task Observer_Records_And_The_Audit_Log_Mark_The_Call_As_An_External_Agent_With_The_OAuth_Client()
    {
        var token = NewUser(TenantA) with { ClientId = "claude-code" };
        await using var client = await ConnectAsync(token);

        (await CallAsync(client, McpSampleAppService.GetNotes)).IsError.ShouldNotBe(true);
        (await CallAsync(client, McpSampleAppService.DeleteNote, new { id = Guid.NewGuid() })).IsError.ShouldBe(true);

        Observer.Records.Count.ShouldBe(2);
        foreach (var record in Observer.Records)
        {
            record.ClientId.ShouldBe("claude-code");
            record.ClientName.ShouldStartWith("MCP external agent: ");
            record.ClientName.ShouldContain("test-agent");
            record.UserId.ShouldBe(token.UserId);
            record.TenantId.ShouldBe(TenantA);
        }

        var log = await WaitForAuditLogAsync(McpSampleAppService.GetNotes);
        log.UserId.ShouldBe(token.UserId);
        log.TenantId.ShouldBe(TenantA);
        log.TenantName.ShouldBe("tenant-a");
        log.ClientId.ShouldBe("claude-code");
        log.Comments.ShouldContain(c => c.StartsWith($"Agent tool {McpSampleAppService.GetNotes} (client: claude-code; MCP external agent: "));
    }

    [Fact]
    public async Task A_Long_User_Agent_Is_Cut_To_128_Characters_In_The_Client_Name()
    {
        var http = CreateHttpClient(NewUser(TenantA));
        var longProduct = "x/" + new string('a', 300);
        http.DefaultRequestHeaders.UserAgent.ParseAdd(longProduct);
        await using var client = await ConnectAsync(http);

        await CallAsync(client, McpSampleAppService.GetNotes);

        Observer.Records.ShouldHaveSingleItem().ClientName.ShouldBe("MCP external agent: " + $"{UserAgent} {longProduct}"[..128]);
    }

    [Fact]
    public async Task The_Cut_Does_Not_Split_A_Surrogate_Pair()
    {
        var http = CreateHttpClient(NewUser(TenantA));
        var userAgent = new string('a', 127) + "😀" + new string('b', 10); // the emoji's high surrogate at index 127
        http.DefaultRequestHeaders.UserAgent.Clear();
        http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", userAgent);
        await using var client = await ConnectAsync(http);

        await CallAsync(client, McpSampleAppService.GetNotes);

        Observer.Records.ShouldHaveSingleItem().ClientName.ShouldBe("MCP external agent: " + new string('a', 127));
    }

    private async Task<Volo.Abp.Auditing.AuditLogInfo> WaitForAuditLogAsync(string toolName)
    {
        // The audit middleware saves after the MCP response has been streamed to the client.
        for (var i = 0; i < 100; i++)
        {
            var log = AuditingStore.Logs.FirstOrDefault(l => l.Comments.Any(c => c.StartsWith($"Agent tool {toolName} ")));
            if (log != null)
            {
                return log;
            }

            await Task.Delay(50);
        }

        throw new ShouldAssertException($"No audit log with a comment for {toolName}.");
    }
}

public class McpToolFilterTests : AgentToolsMcpTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpAgentToolsMcpOptions>(o => o.ToolFilter = d => d.Name != McpSampleAppService.CreateNote);
    }

    [Theory]
    [InlineData("en", "There is no agent tool named")]
    [InlineData("tr", "adında bir ajan aracı yok")]
    public async Task Tool_Left_Out_By_The_Filter_Is_Absent_And_Refused_Exactly_Like_An_Unknown_Tool(string culture, string expected)
    {
        var http = CreateHttpClient(NewUser(TenantA));
        http.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
        await using var client = await ConnectAsync(http);

        (await client.ListToolsAsync()).Select(t => t.Name).ShouldBe([McpSampleAppService.GetNotes]);
        var filtered = await CallAsync(client, McpSampleAppService.CreateNote, new { input = new { text = "hello" } });
        var unknown = await CallAsync(client, "McpSample_Nope");

        filtered.IsError.ShouldBe(true);
        Text(filtered).ShouldContain(expected);
        Text(filtered).ShouldBe(Text(unknown).Replace("McpSample_Nope", McpSampleAppService.CreateNote));
        var hidden = Observer.Records.Single(r => r.ToolName == McpSampleAppService.CreateNote);
        var nope = Observer.Records.Single(r => r.ToolName == "McpSample_Nope");
        hidden.Succeeded.ShouldBeFalse();
        hidden.ErrorType.ShouldBe(nope.ErrorType);
        hidden.ErrorCode.ShouldBe(AgentToolsErrorCodes.ToolNotFound);
        hidden.ErrorCode.ShouldBe(nope.ErrorCode);
        hidden.ErrorMessage.ShouldBe(nope.ErrorMessage!.Replace("McpSample_Nope", McpSampleAppService.CreateNote));
        hidden.ClientId.ShouldBe(nope.ClientId);
        hidden.ClientName.ShouldBe(nope.ClientName);
        hidden.ClientName.ShouldStartWith("MCP external agent");
        hidden.UserId.ShouldBe(nope.UserId);
        hidden.TenantId.ShouldBe(nope.TenantId);
    }

    [Fact]
    public async Task With_The_Feature_Off_The_Filtered_Tool_Is_Refused_Like_An_Unknown_Tool()
    {
        FeatureStore.SetForTenant(TenantA, AgentToolsFeatures.Enable, "false");
        await using var client = await ConnectAsync(NewUser(TenantA));

        var filtered = await CallAsync(client, McpSampleAppService.CreateNote, new { input = new { text = "hello" } });

        Text(filtered).ShouldBe(Text(await CallAsync(client, "McpSample_Nope")));
        Text(filtered).ShouldContain(AgentToolsErrorCodes.FeatureDisabled);
    }
}
