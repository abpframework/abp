using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Shouldly;
using Volo.Abp.AgentTools.Sample;
using Volo.Abp.Authorization;
using Xunit;

namespace Volo.Abp.AgentTools;

public class AgentToolProviderTests : AgentToolsInvocationTestBase
{
    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly Guid TenantA = Guid.NewGuid();
    private static readonly Guid TenantB = Guid.NewGuid();

    private async Task<Dictionary<string, AgentTool>> GetToolsAsync(AgentToolContext? context = null)
    {
        return (await Provider.GetToolsAsync(context ?? WithApproval)).ToDictionary(t => t.Definition.Name);
    }

    [Theory]
    [InlineData("Book_Create", SamplePermissions.Books.Create, "name")]               // unproxied service
    [InlineData("SampleReport_Create", SamplePermissions.Reports.Create, "code")]     // proxied service, [Authorize] on the implementation
    public async Task Tool_Without_Permission_Is_Absent_And_Refused_By_ABP_Then_Present_With_The_Grant(string toolName, string permission, string property)
    {
        using var _ = LoginAs(UserId);
        var arguments = new { input = new Dictionary<string, object> { [property] = "P-1" } };

        var tools = await GetToolsAsync();
        tools.ShouldNotContainKey(toolName);
        tools.ShouldContainKey("Book_GetList");
        (await InvokeAsync(toolName, arguments)).ShouldStartWith($"ERROR {nameof(AbpAuthorizationException)}: ");

        PermissionStore.GrantToUser(UserId, permission);

        (await GetToolsAsync()).ShouldContainKey(toolName);
        Json(await InvokeAsync(toolName, arguments));
    }

    [Fact]
    public async Task Authorize_On_A_Hand_Written_Controller_Action_Is_Not_Bypassed_In_Process()
    {
        // [Authorize(Reports.Delete)] sits only on SampleReportController.DeleteAsync; interface and implementation have none.
        using var _ = LoginAs(UserId);
        var arguments = new { id = Guid.NewGuid() };

        (await GetToolsAsync()).ShouldNotContainKey("SampleReport_Delete");
        (await InvokeAsync("SampleReport_Delete", arguments)).ShouldStartWith($"ERROR {nameof(AbpAuthorizationException)}: ");

        PermissionStore.GrantToUser(UserId, SamplePermissions.Reports.Delete);

        (await GetToolsAsync()).ShouldContainKey("SampleReport_Delete");
        (await InvokeAsync("SampleReport_Delete", arguments)).ShouldBe(AgentToolInvoker.VoidResultJson);
    }

    [Fact]
    public async Task Tool_Disabled_In_The_Policy_Store_Is_Absent_And_Refused_By_Name()
    {
        PolicyStore.Set("Book_GetList", null, enabled: false, requiresApproval: null);

        (await GetToolsAsync()).ShouldNotContainKey("Book_GetList");
        (await InvokeAsync("Book_GetList")).ShouldBe(
            $"ERROR BusinessException: The agent tool 'Book_GetList' is disabled. (code: {AgentToolsErrorCodes.ToolDisabled})");
    }

    [Fact]
    public async Task Approval_Required_Tool_Is_Excluded_Without_Approval_Support_And_Marked_With_It()
    {
        var withoutApproval = await GetToolsAsync(WithoutApproval);
        withoutApproval.ShouldNotContainKey("Book_GetCount"); // [AgentTool(RequiresApproval = true)]
        withoutApproval.Values.ShouldAllBe(t => !t.RequiresApproval);

        var withApproval = await GetToolsAsync(WithApproval);
        var count = withApproval["Book_GetCount"];
        count.RequiresApproval.ShouldBeTrue();
#pragma warning disable MEAI001 // ApprovalRequiredAIFunction is experimental
        count.Function.ShouldBeOfType<ApprovalRequiredAIFunction>().Name.ShouldBe("Book_GetCount");
        count.UnwrappedFunction.ShouldBeOfType<AgentToolFunction>();
#pragma warning restore MEAI001

        var list = withApproval["Book_GetList"];
        list.RequiresApproval.ShouldBeFalse();
        list.Function.ShouldBeSameAs(list.UnwrappedFunction);
    }

    [Fact]
    public async Task Function_From_A_Stale_List_Is_Refused_When_The_Tool_Now_Requires_Approval()
    {
        var list = (await GetToolsAsync(WithApproval))["Book_GetList"];
        list.RequiresApproval.ShouldBeFalse();

        PolicyStore.Set("Book_GetList", null, enabled: null, requiresApproval: true);

        (await list.Function.InvokeAsync(Args())).ShouldBe(
            "ERROR BusinessException: The agent tool 'Book_GetList' now requires human approval, but it was listed without it. " +
            $"Get the tool list again. (code: {AgentToolsErrorCodes.ApprovalPolicyChanged})");

        // A fresh list marks it, and its (approval-wrapped) function runs once the host has asked.
        var fresh = (await GetToolsAsync(WithApproval))["Book_GetList"];
        fresh.RequiresApproval.ShouldBeTrue();
        Json((string)(await fresh.UnwrappedFunction.InvokeAsync(Args()))!);
    }

    [Fact]
    public async Task Tool_Disabled_For_One_Tenant_Is_Absent_And_Refused_There_Only()
    {
        PolicyStore.Set("Book_GetList", TenantA, enabled: false, requiresApproval: null);

        (await GetToolsAsync()).ShouldContainKey("Book_GetList");

        using (ChangeTenant(TenantA))
        {
            (await GetToolsAsync()).ShouldNotContainKey("Book_GetList");
            (await GetToolsAsync()).ShouldContainKey("Book_Get");
            (await InvokeAsync("Book_GetList")).ShouldStartWith("ERROR BusinessException: The agent tool 'Book_GetList' is disabled.");
        }

        using (ChangeTenant(TenantB))
        {
            (await GetToolsAsync()).ShouldContainKey("Book_GetList");
        }
    }

    // AIFunction path

    [Fact]
    public async Task Function_Has_Schema_And_Annotations_And_Invokes_Through_The_Invoker()
    {
        var tools = await GetToolsAsync();
        var register = tools["OtherSample_Register"].UnwrappedFunction;

        register.Name.ShouldBe("OtherSample_Register");
        register.Description.ShouldNotBeNullOrWhiteSpace();
        var schema = register.JsonSchema;
        var properties = schema.GetProperty("properties");
        properties.TryGetProperty("input", out var input).ShouldBeTrue();
        input.GetProperty("properties").TryGetProperty("password", out _).ShouldBeTrue();
        properties.TryGetProperty("cancellationToken", out _).ShouldBeFalse();
        schema.GetProperty("required").EnumerateArray().Select(e => e.GetString()).ShouldBe(["input"]);

        tools["Book_Get"].Function.JsonSchema.GetProperty("required").EnumerateArray().Select(e => e.GetString()).ShouldBe(["id"]);
        tools["Book_GetList"].Function.JsonSchema.TryGetProperty("required", out _).ShouldBeFalse();

        var list = tools["Book_GetList"].Function.AdditionalProperties;
        (list["readOnlyHint"], list["destructiveHint"], list["idempotentHint"]).ShouldBe((true, false, true));

        var result = await register.InvokeAsync(Args(new { input = new { userName = "amy", password = "secret-pw" } }));

        JsonSerializer.Deserialize<string>(result.ShouldBeOfType<string>()).ShouldBe("registered amy");
        Observer.Records.ShouldHaveSingleItem().Arguments.ShouldNotContain("secret-pw");
    }
}
