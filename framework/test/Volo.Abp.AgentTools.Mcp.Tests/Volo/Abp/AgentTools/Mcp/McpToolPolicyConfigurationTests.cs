using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using Volo.Abp.AgentTools.Mcp.Sample;
using Xunit;

namespace Volo.Abp.AgentTools.Mcp;

/// <summary>The default policy store with an "AgentTools:Tools" section, over MCP (list and call), for the host and the tenants.</summary>
public class McpToolPolicyConfigurationTests : AgentToolsMcpTestBase
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureAppConfiguration((_, configuration) => configuration.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(
            """
            {
              "AgentTools": {
                "Tools": {
                  "McpSample_GetSecret": { "Enabled": false },
                  "McpSample_GetNotes": { "RequiresApproval": true },
                  "mcpsample_deletenote": { "RequiresApproval": false }
                }
              }
            }
            """))));
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<IAgentToolPolicyStore, ConfigurationAgentToolPolicyStore>());
    }

    public static TheoryData<Guid?> HostAndTenants => [null, TenantA, TenantB];

    [Theory]
    [MemberData(nameof(HostAndTenants))]
    public async Task List_Follows_The_Configuration(Guid? tenantId)
    {
        await using var client = await ConnectAsync(NewUser(tenantId, McpSamplePermissions.Secret));

        // Without GetSecret (disabled) and GetNotes (a read that requires approval), with DeleteNote (no approval any more);
        // CreateNote and GetDisabled have no settings.
        (await client.ListToolsAsync()).Select(t => t.Name).OrderBy(n => n)
            .ShouldBe([McpSampleAppService.CreateNote, McpSampleAppService.DeleteNote, McpSampleAppService.GetDisabled]);
    }

    [Theory]
    [MemberData(nameof(HostAndTenants))]
    public async Task Call_Follows_The_Configuration(Guid? tenantId)
    {
        await using var client = await ConnectAsync(NewUser(tenantId, McpSamplePermissions.Secret));

        var disabled = await CallAsync(client, McpSampleAppService.GetSecret);
        disabled.IsError.ShouldBe(true);
        Text(disabled).ShouldBe($"ERROR BusinessException: The agent tool '{McpSampleAppService.GetSecret}' is disabled. (code: {AgentToolsErrorCodes.ToolDisabled})");

        var approval = await CallAsync(client, McpSampleAppService.GetNotes);
        approval.IsError.ShouldBe(true);
        Text(approval).ShouldContain(AgentToolsErrorCodes.ApprovalNotSupported);

        var created = await CallAsync(client, McpSampleAppService.CreateNote, new { input = new { text = "hello" } });
        created.IsError.ShouldNotBe(true);
        var id = JsonSerializer.Deserialize<JsonElement>(Text(created)).GetProperty("id").GetGuid();
        var deleted = await CallAsync(client, McpSampleAppService.DeleteNote, new { id });
        deleted.IsError.ShouldNotBe(true, Text(deleted));
    }
}
