using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AgentTools.SchemaSample;
using Xunit;

namespace Volo.Abp.AgentTools;

/// <summary>Required-ness, DataAnnotations and enums in the chat tool schema, enum arguments and results.</summary>
public class AgentToolSchemaTests : AgentToolsInvocationTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.Configure<AbpAgentToolOptions>(o => o.IncludedRemoteServiceNames.Add(SchemaSampleAppService.RemoteServiceName));
    }

    private async Task<AgentTool> GetToolAsync(string name) => (await Provider.GetToolsAsync(WithApproval)).Single(t => t.Definition.Name == name);

    [Fact]
    public async Task Chat_Schema_Has_Required_Properties_And_DataAnnotations_For_Flat_Nested_And_Collection_Dtos()
    {
        var tool = await GetToolAsync(SchemaSampleAppService.GetEcho);

        SchemaSampleAssertions.ShouldDescribeTheEchoInput(tool.UnwrappedFunction.JsonSchema);
        tool.Function.JsonSchema.GetRawText().ShouldBe(tool.UnwrappedFunction.JsonSchema.GetRawText());
    }

    [Fact]
    public void Non_Nullable_Value_Types_Are_Not_Required_When_The_Option_Is_Off()
    {
        var definition = CreateCatalog(o =>
        {
            o.IncludedRemoteServiceNames.Add(SchemaSampleAppService.RemoteServiceName);
            o.RequireNonNullableValueTypes = false;
        }).Find(SchemaSampleAppService.GetEcho)!;

        definition.RequireNonNullableValueTypes.ShouldBeFalse();
        SchemaSampleAssertions.ShouldDescribeTheEchoInput(AgentToolJsonSchema.Get(definition), requireNonNullableValueTypes: false);
    }

    [Fact]
    public async Task Chat_Schema_Leaves_Abp_Paging_And_Sorting_Members_Optional_Unless_Explicitly_Required()
    {
        SchemaSampleAssertions.ShouldHaveOptionalPagingMembers((await GetToolAsync(SchemaSampleAppService.GetPage)).Function.JsonSchema);
    }

    [Fact]
    public async Task Paged_Input_Without_Paging_Arguments_Is_Accepted()
    {
        (await InvokeAsync(SchemaSampleAppService.GetPage, new { input = new { filter = "x" } })).ShouldBe("0");
    }

    [Fact]
    public async Task Enum_Parameter_Schema_Lists_The_Names()
    {
        var schema = (await GetToolAsync(SchemaSampleAppService.GetNextColor)).Function.JsonSchema;

        var color = schema.GetProperty("properties").GetProperty("color");
        color.GetProperty("type").GetString().ShouldBe("string");
        color.GetProperty("enum").EnumerateArray().Select(e => e.GetString()).ShouldBe(["Red", "Green", "Blue"]);
    }

    [Theory]
    [InlineData("Green")]
    [InlineData("green")]
    [InlineData(1)]
    public async Task Enum_Argument_Is_Accepted_As_Name_Or_Number_And_The_Result_Carries_The_Name(object color)
    {
        (await InvokeAsync(SchemaSampleAppService.GetNextColor, new { color })).ShouldBe("\"Blue\"");
    }

    [Fact]
    public async Task Enums_Inside_Dto_Arguments_And_Results_Travel_As_Names_And_Numbers_Are_Still_Read()
    {
        var result = Json(await InvokeAsync(SchemaSampleAppService.GetEcho, new
        {
            input = new
            {
                name = "ab",
                code = "c1",
                color = "Blue",
                lines = new object[] { new { sku = "s1", unit = "pc", quantity = 1, color = 0 }, new { sku = "s2", unit = "pc", quantity = 2 } }
            }
        }));

        result.GetProperty("color").GetString().ShouldBe("Blue");
        result.GetProperty("lineColors").EnumerateArray().Select(e => e.GetString()).ShouldBe(["Red", "Green"]);
    }
}
