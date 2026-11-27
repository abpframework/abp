using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Volo.Abp.AgentTools.Sample;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.DynamicProxy;
using Xunit;

namespace Volo.Abp.AgentTools;

public class AgentToolInvokerTests : AgentToolsInvocationTestBase
{
    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly Guid TenantA = Guid.NewGuid();
    private static readonly Guid TenantB = Guid.NewGuid();

    private async Task<string[]> ListBookNamesAsync()
    {
        return Json(await InvokeAsync("Book_GetList")).EnumerateArray().Select(b => b.GetProperty("name").GetString()!).ToArray();
    }

    // Services exposed as conventional controllers through PreConfigure<AbpAspNetCoreMvcOptions> are not proxied by ABP (MVC filters
    // do the cross-cutting work for HTTP calls), so the invoker does it; services behind hand-written controllers are proxied.
    [Fact]
    public void Conventional_Controller_Service_Is_Not_Proxied_But_Hand_Written_Controller_Service_Is()
    {
        ProxyHelper.IsProxy(GetRequiredService<IBookAppService>()).ShouldBeFalse();
        ProxyHelper.IsProxy(GetRequiredService<ISampleReportAppService>()).ShouldBeTrue();
    }

    [Fact]
    public async Task Success_Returns_The_Method_Result_As_Json()
    {
        PermissionStore.GrantToUser(UserId, SamplePermissions.Books.Create);
        using var _ = LoginAs(UserId);

        var created = Json(await InvokeAsync("Book_Create", new { input = new { name = "Dune" } }));
        created.GetProperty("name").GetString().ShouldBe("Dune");
        var id = created.GetProperty("id").GetGuid();
        id.ShouldNotBe(Guid.Empty);

        Json(await InvokeAsync("Book_Get", new { id })).GetProperty("name").GetString().ShouldBe("Dune");
        Json(await InvokeAsync("SampleReport_Get_2", new { code = "R-7" })).GetProperty("code").GetString().ShouldBe("R-7");
        (await InvokeAsync("Book_Archive", new { id })).ShouldBe(AgentToolInvoker.VoidResultJson);
        (await ListBookNamesAsync()).ShouldBeEmpty();
    }

    [Theory]
    [InlineData("SampleReport_Create", "code")] // proxied: ABP's AuthorizationInterceptor refuses
    [InlineData("Book_Create", "name")]         // unproxied: the invoker runs IMethodInvocationAuthorizationService itself
    public async Task Authorization_Failure_Returns_AbpAuthorizationException(string toolName, string property)
    {
        using var _ = LoginAs(UserId);

        var result = await InvokeAsync(toolName, new { input = new System.Collections.Generic.Dictionary<string, object> { [property] = "X-1" } });

        result.ShouldStartWith($"ERROR {nameof(AbpAuthorizationException)}: ");
        (await ListBookNamesAsync()).ShouldBeEmpty();
    }

    [Theory]
    [InlineData("SampleReport_Create", "code", SamplePermissions.Reports.Create)] // proxied: ABP's ValidationInterceptor
    [InlineData("Book_Create", "name", SamplePermissions.Books.Create)]           // unproxied: IMethodInvocationValidator run by the invoker
    public async Task Validation_Failure_Returns_AbpValidationException_With_The_Field_Messages(string toolName, string property, string permission)
    {
        PermissionStore.GrantToUser(UserId, permission);
        using var _ = LoginAs(UserId);

        var result = await InvokeAsync(toolName, new { input = new System.Collections.Generic.Dictionary<string, object> { [property] = new string('x', 200) } });

        result.ShouldStartWith("ERROR AbpValidationException: ");
        result.ShouldContain("Validation errors: [" + char.ToUpperInvariant(property[0]) + property[1..] + "]");
        result.ShouldContain("maximum length");
    }

    [Fact]
    public async Task Missing_Required_Argument_Returns_AbpValidationException()
    {
        PermissionStore.GrantToUser(UserId, SamplePermissions.Books.Create);
        using var _ = LoginAs(UserId);

        var result = await InvokeAsync("Book_Create");

        result.ShouldStartWith("ERROR AbpValidationException: ");
        result.ShouldContain("[input] The input field is required.");
    }

    [Fact]
    public async Task Argument_That_Does_Not_Convert_Returns_AbpValidationException_Naming_The_Parameter()
    {
        var result = await InvokeAsync("Book_Get", new { id = "not-a-guid" });

        result.ShouldStartWith("ERROR AbpValidationException: ");
        result.ShouldContain("[id] The id argument is not a valid Guid");
    }

    [Fact]
    public async Task Cancelled_Call_Is_Recorded_Once_And_Rethrown()
    {
        using var cancelled = new System.Threading.CancellationTokenSource();
        await cancelled.CancelAsync();

        await Should.ThrowAsync<OperationCanceledException>(() => Invoker.InvokeAsync("Book_GetList", Args(), WithApproval, cancelled.Token));

        var record = Observer.Records.ShouldHaveSingleItem();
        record.Succeeded.ShouldBeFalse();
        record.ErrorType.ShouldBe(typeof(OperationCanceledException).FullName);
    }

    [Fact]
    public async Task Business_Exception_Returns_Its_Code_And_Message()
    {
        var result = await InvokeAsync("Book_CreateThenFail", new { input = new { name = "Doomed" } });

        result.ShouldBe($"ERROR BusinessException: The book was saved, then the operation failed. (code: {BookAppService.CreateThenFailErrorCode})");
    }

    [Fact]
    public async Task An_Exception_Rolls_Back_The_Calls_Unit_Of_Work()
    {
        (await InvokeAsync("Book_CreateThenFail", new { input = new { name = "Rolled back" } })).ShouldStartWith("ERROR BusinessException");

        (await ListBookNamesAsync()).ShouldNotContain("Rolled back");
    }

    [Fact]
    public async Task Unknown_And_Approval_Required_Tools_Are_Refused_By_Name()
    {
        (await InvokeAsync("Book_Nope")).ShouldBe(
            $"ERROR BusinessException: There is no agent tool named 'Book_Nope'. (code: {AgentToolsErrorCodes.ToolNotFound})");

        (await InvokeAsync("Book_GetCount", context: WithoutApproval)).ShouldBe(
            "ERROR BusinessException: The agent tool 'Book_GetCount' requires human approval, which this client does not support. " +
            $"(code: {AgentToolsErrorCodes.ApprovalNotSupported})");

        Json(await InvokeAsync("Book_GetCount", context: WithApproval)).GetInt32().ShouldBe(0);
    }

    [Theory]
    [InlineData("Book_Nope\r\nBook_GetList (client: other)", "Book_Nope??Book_GetList (client: other)")]
    [InlineData("a\tb\0c\u001bd\u007fe\u0085f\u2028g\u2029h", "a?b?c?d?e?f?g?h")]
    public async Task Control_Characters_In_The_Name_Of_An_Unknown_Tool_Are_Replaced_In_Every_Output(string name, string shown)
    {
        await ShouldBeRefusedAsUnknownAsync(name, shown);
    }

    [Fact]
    public async Task The_Name_Of_An_Unknown_Tool_Is_Cut_In_Every_Output()
    {
        await ShouldBeRefusedAsUnknownAsync(new string('x', 100_000), new string('x', 128));
        await ShouldBeRefusedAsUnknownAsync(new string('x', 128), new string('x', 128));
        // Not inside a surrogate pair.
        await ShouldBeRefusedAsUnknownAsync(new string('x', 127) + "\U0001F600", new string('x', 127));
    }

    [Theory]
    [InlineData("Book_GetList\n")]
    [InlineData("\0Book_GetList")]
    public async Task A_Name_That_Is_A_Tool_Name_Without_Its_Control_Characters_Is_Unknown(string name)
    {
        (await InvokeAsync(name)).ShouldContain(AgentToolsErrorCodes.ToolNotFound);
    }

    [Fact]
    public async Task The_Refusal_Exception_Carries_The_Shown_Name_Only()
    {
        var invoker = new RefusalInvoker(GetRequiredService<IAgentToolCatalog>(), GetRequiredService<IServiceScopeFactory>(), InvokerLogger);

        await invoker.InvokeAsync("x\ny", Args(), WithApproval);

        // The logger gets the exception itself.
        invoker.Refusal!.Data["ToolName"].ShouldBe("x?y");
        invoker.Refusal.Message.ShouldBe("There is no agent tool named 'x?y'.");
    }

    private class RefusalInvoker(IAgentToolCatalog catalog, IServiceScopeFactory scopeFactory, ILogger<AgentToolInvoker> logger)
        : AgentToolInvoker(catalog, scopeFactory, logger)
    {
        public Exception? Refusal { get; private set; }

        protected override string FormatErrorMessage(IServiceProvider services, Exception exception)
        {
            Refusal = exception;
            return base.FormatErrorMessage(services, exception);
        }
    }

    private async Task ShouldBeRefusedAsUnknownAsync(string name, string shown)
    {
        InvokerLogger.Messages.Clear();
        Observer.Records.Clear();
        var auditingManager = GetRequiredService<IAuditingManager>();
        using var scope = auditingManager.BeginScope();

        var result = await InvokeAsync(name);

        result.ShouldBe($"ERROR BusinessException: There is no agent tool named '{shown}'. (code: {AgentToolsErrorCodes.ToolNotFound})");
        InvokerLogger.Messages.ShouldContain(m => m.StartsWith($"Agent tool {shown} failed in "));
        InvokerLogger.Messages.ShouldAllBe(m => m.Length < 1000 && !m.Any(char.IsControl) && !m.Contains('\u2028') && !m.Contains('\u2029'));
        auditingManager.Current!.Log.Comments.ShouldHaveSingleItem().ShouldBe($"Agent tool {shown} (client: test-client; Test Client)");
        var record = Observer.Records.ShouldHaveSingleItem();
        record.ToolName.ShouldBe(shown);
        record.ErrorMessage.ShouldBe($"There is no agent tool named '{shown}'. (code: {AgentToolsErrorCodes.ToolNotFound})");
    }

    [Fact]
    public async Task The_Same_Tool_Under_Two_Tenants_Sees_Only_Its_Tenants_Data()
    {
        PermissionStore.GrantToUser(UserId, SamplePermissions.Books.Create);

        foreach (var (tenant, name) in new[] { (TenantA, "A-book"), (TenantB, "B-book") })
        {
            using (LoginAs(UserId, tenant))
            using (ChangeTenant(tenant))
            {
                Json(await InvokeAsync("Book_Create", new { input = new { name } }));
            }
        }

        using (LoginAs(UserId, TenantA))
        using (ChangeTenant(TenantA))
        {
            (await ListBookNamesAsync()).ShouldBe(["A-book"]);
        }

        using (LoginAs(UserId, TenantB))
        using (ChangeTenant(TenantB))
        {
            (await ListBookNamesAsync()).ShouldBe(["B-book"]);
        }

        (await ListBookNamesAsync()).ShouldBeEmpty(); // host
    }

    [Fact]
    public async Task Names_Added_Through_The_Option_Are_Redacted_In_The_Record_And_The_Log()
    {
        GetRequiredService<Microsoft.Extensions.Options.IOptions<AbpAgentToolOptions>>().Value.AdditionalSensitiveArgumentNames.Add("endpoint");
        using var login = LoginAs(UserId, TenantA, "agent-user");
        using var tenant = ChangeTenant(TenantA);

        await InvokeAsync("OtherSample_Register", new
        {
            input = new { userName = "bob", password = "hunter2", api = new { endpoint = "https://example.test", apiKey = "k-123" } }
        });

        var record = Observer.Records.ShouldHaveSingleItem();
        record.Arguments.ShouldNotContain("example.test");
        record.Arguments.ShouldContain("\"endpoint\":\"***\"");
        record.Arguments.ShouldContain("\"userName\":\"bob\"");
        InvokerLogger.Messages.ShouldNotContain(m => m.Contains("example.test"));
    }

    [Fact]
    public async Task Observer_Gets_One_Record_Per_Call_With_Redacted_Arguments()
    {
        using var login = LoginAs(UserId, TenantA, "agent-user");
        using var tenant = ChangeTenant(TenantA);

        var result = await InvokeAsync("OtherSample_Register", new
        {
            input = new { userName = "bob", password = "hunter2", api = new { endpoint = "https://example.test", apiKey = "k-123" } }
        });

        Json(result).GetString().ShouldBe("registered bob"); // the throwing observer registered first did not break the call
        var record = Observer.Records.ShouldHaveSingleItem();
        record.ToolName.ShouldBe("OtherSample_Register");
        record.Succeeded.ShouldBeTrue();
        record.ErrorType.ShouldBeNull();
        record.Arguments.ShouldNotContain("hunter2");
        record.Arguments.ShouldNotContain("k-123");
        record.Arguments.ShouldContain("\"password\":\"***\"");
        record.Arguments.ShouldContain("\"apiKey\":\"***\"");
        record.Arguments.ShouldContain("\"userName\":\"bob\"");
        record.Duration.ShouldBeGreaterThan(TimeSpan.Zero);
        record.StartTime.ShouldNotBe(default);
        record.UserId.ShouldBe(UserId);
        record.UserName.ShouldBe("agent-user");
        record.TenantId.ShouldBe(TenantA);
        record.ClientId.ShouldBe("test-client");

        InvokerLogger.Messages.ShouldContain(m => m.Contains("OtherSample_Register") && m.Contains("\"password\":\"***\""));
        InvokerLogger.Messages.ShouldNotContain(m => m.Contains("hunter2") || m.Contains("k-123"));

        (await InvokeAsync("Book_Create", new { input = new { name = "x" } })).ShouldStartWith("ERROR");
        Observer.Records.Count.ShouldBe(2);
        var failed = Observer.Records.Last();
        failed.Succeeded.ShouldBeFalse();
        failed.ErrorType.ShouldBe(typeof(AbpAuthorizationException).FullName);
        failed.ErrorMessage.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Duplicate_Argument_Names_Are_Recorded_And_Redacted()
    {
        var arguments = Args();
        arguments["input"] = JsonSerializer.Deserialize<JsonElement>(
            """{"userName":"bob","UserName":"alice","password":"hunter1","password":"hunter2"}""");
        arguments["unknown"] = JsonSerializer.Deserialize<JsonElement>("""{"a":1,"a":2}""");

        Json(await Invoker.InvokeAsync("OtherSample_Register", arguments, WithApproval)).GetString().ShouldBe("registered alice");

        var recorded = Observer.Records.ShouldHaveSingleItem().Arguments;
        recorded.ShouldContain("""{"userName":"bob","UserName":"alice","password":"***","password":"***"}""");
        recorded.ShouldContain("""{"a":1,"a":2}""");
        InvokerLogger.Messages.ShouldContain(m => m.StartsWith("Agent tool OtherSample_Register succeeded") && m.Contains(recorded));
        InvokerLogger.Messages.ShouldNotContain(m => m.Contains("hunter") || m.Contains("could not be serialized"));
    }

    [Fact]
    public async Task A_Json_String_Is_Redacted_In_The_Record_And_The_Log_And_The_Call_Gets_The_Original()
    {
        const string json = """{"id":1,"password":"hunter2"}""";

        Json(await InvokeAsync("OtherSample_Register", new { input = new { userName = json, password = "hunter1" } }))
            .GetString().ShouldBe("registered " + json);

        var recorded = Observer.Records.ShouldHaveSingleItem().Arguments;
        JsonSerializer.Deserialize<JsonElement>(recorded).GetProperty("input").GetProperty("userName").GetString()
            .ShouldBe("""{"id":1,"password":"***"}""");
        InvokerLogger.Messages.ShouldContain(m => m.StartsWith("Agent tool OtherSample_Register succeeded") && m.Contains(recorded));

        // The whole input as one string: the call fails.
        (await InvokeAsync("OtherSample_Register", new { input = json })).ShouldStartWith("ERROR");

        recorded = Observer.Records.Last().Arguments;
        JsonSerializer.Deserialize<JsonElement>(recorded).GetProperty("input").GetString().ShouldBe("""{"id":1,"password":"***"}""");
        InvokerLogger.Messages.ShouldContain(m => m.StartsWith("Agent tool OtherSample_Register failed") && m.Contains(recorded));
        InvokerLogger.Messages.ShouldNotContain(m => m.Contains("hunter"));
    }

    [Fact]
    public async Task Audit_Log_Gets_The_Agent_Comment_And_The_Action_On_Both_Paths()
    {
        var auditingManager = GetRequiredService<IAuditingManager>();
        using var scope = auditingManager.BeginScope();

        Json(await InvokeAsync("Book_GetList"));          // unproxied: action added by the invoker
        Json(await InvokeAsync("SampleReport_Get_2", new { code = "R-1" })); // proxied: action added by ABP's AuditingInterceptor

        var log = auditingManager.Current!.Log;
        log.Comments.ShouldContain("Agent tool Book_GetList (client: test-client; Test Client)");
        log.Comments.ShouldContain("Agent tool SampleReport_Get_2 (client: test-client; Test Client)");
        log.Actions.ShouldContain(a => a.ServiceName == typeof(BookAppService).FullName && a.MethodName == nameof(IBookAppService.GetListAsync));
        log.Actions.ShouldContain(a => a.ServiceName == typeof(SampleReportAppService).FullName && a.MethodName == nameof(ISampleReportAppService.GetAsync));
    }
}
