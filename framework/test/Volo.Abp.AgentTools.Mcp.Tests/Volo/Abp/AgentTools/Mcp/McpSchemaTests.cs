using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AgentTools.SchemaSample;
using Xunit;

namespace Volo.Abp.AgentTools.Mcp;

/// <summary>Over MCP: the tools/list input schema and enum arguments and results.</summary>
public class McpSchemaTests : AgentToolsMcpTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpAgentToolOptions>(o => o.IncludedRemoteServiceNames.Add(SchemaSampleAppService.RemoteServiceName));
    }

    [Fact]
    public async Task Tools_List_Input_Schema_Has_Required_Properties_And_DataAnnotations_For_Flat_Nested_And_Collection_Dtos()
    {
        await using var client = await ConnectAsync(NewUser(TenantA));

        var tools = (await client.ListToolsAsync()).Select(t => t.ProtocolTool).ToList();

        var echo = tools.Single(t => t.Name == SchemaSampleAppService.GetEcho);
        SchemaSampleAssertions.ShouldDescribeTheEchoInput(echo.InputSchema);
        var nextColor = tools.Single(t => t.Name == SchemaSampleAppService.GetNextColor).InputSchema.GetProperty("properties").GetProperty("color");
        nextColor.GetProperty("enum").EnumerateArray().Select(e => e.GetString()).ShouldBe(["Red", "Green", "Blue"]);
    }

    [Fact]
    public async Task Tools_List_Input_Schema_Leaves_Abp_Paging_And_Sorting_Members_Optional_Unless_Explicitly_Required()
    {
        await using var client = await ConnectAsync(NewUser(TenantA));

        var page = (await client.ListToolsAsync()).Single(t => t.Name == SchemaSampleAppService.GetPage);
        SchemaSampleAssertions.ShouldHaveOptionalPagingMembers(page.ProtocolTool.InputSchema);
    }

    [Fact]
    public async Task Enum_Argument_Is_Accepted_As_Name_Or_Number_And_The_Result_Carries_The_Name()
    {
        await using var client = await ConnectAsync(NewUser(TenantA));

        Text(await CallAsync(client, SchemaSampleAppService.GetNextColor, new { color = "Green" })).ShouldBe("\"Blue\"");
        Text(await CallAsync(client, SchemaSampleAppService.GetNextColor, new { color = 2 })).ShouldBe("\"Red\"");

        var echo = await CallAsync(client, SchemaSampleAppService.GetEcho,
            new { input = new { name = "ab", code = "c1", color = 1, lines = new[] { new { sku = "s1", unit = "pc", quantity = 1 } } } });
        echo.IsError.ShouldBe(false, Text(echo));
        var result = JsonSerializer.Deserialize<JsonElement>(Text(echo));
        result.GetProperty("color").GetString().ShouldBe("Green");
        result.GetProperty("lineColors").EnumerateArray().Select(e => e.GetString()).ShouldBe(["Green"]);
    }
}
