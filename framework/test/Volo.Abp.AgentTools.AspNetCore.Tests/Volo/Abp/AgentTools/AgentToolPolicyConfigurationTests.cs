using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Volo.Abp.AgentTools.Sample;
using Volo.Abp.AgentTools.Sample.Diagnostics;
using Xunit;

namespace Volo.Abp.AgentTools;

/// <summary>Host with the given "AgentTools:Tools" section (null = none) and the default policy store.</summary>
public abstract class AgentToolPolicyConfigurationTestBase : AgentToolsAspNetCoreTestBase
{
    protected abstract string? Tools { get; }

    /// <summary>The whole configuration file (null = none).</summary>
    protected virtual string? Json => Tools == null ? null : "{\"AgentTools\":{\"Tools\":" + Tools + "}}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        if (Json != null)
        {
            builder.ConfigureAppConfiguration((_, configuration) =>
                configuration.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(Json))));
        }
    }
}

// Provider and invoker.
public class AgentToolPolicyConfigurationTests : AgentToolPolicyConfigurationTestBase
{
    private static readonly AgentToolContext WithApproval = new() { SupportsApproval = true };
    private static readonly AgentToolContext WithoutApproval = new() { SupportsApproval = false };

    protected override string Tools =>
        """
        {
          "Book_GetList": { "Enabled": false },
          "book_find": { "enabled": false },
          "Book_Get": { "RequiresApproval": true },
          "Book_Update": { "RequiresApproval": false },
          "Book_CreateThenFail": { "Enabled": true },
          "Book_Archive": {},
          "Book_GetCount": null,
          "OtherSample_Register": { "RequiresApproval": false }
        }
        """;

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpAgentToolOptions>(o =>
        {
            o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample);
            o.IncludedRemoteServiceNames.Add(SampleRemoteServices.OtherSample);
        });
        services.Configure<AbpAgentToolPolicyOptions>(o =>
            o.Tools["OtherSample_Register"] = new AgentToolPolicyConfiguration { Enabled = false, RequiresApproval = true });
    }

    public static TheoryData<Guid?> HostAndTenants => [null, Guid.NewGuid(), Guid.NewGuid()];

    [Theory]
    [MemberData(nameof(HostAndTenants))]
    public async Task Disabled_Tool_Is_Not_Listed_And_Refused_By_Name_For_The_Host_And_Any_Tenant(Guid? tenantId)
    {
        using var _ = ChangeTenant(tenantId);

        // Book_Find: configured in another case.
        foreach (var toolName in new[] { "Book_GetList", "Book_Find" })
        {
            (await ListAsync(WithApproval)).ShouldNotContainKey(toolName);
            (await InvokeAsync(toolName, WithApproval)).ShouldBe(
                $"ERROR BusinessException: The agent tool '{toolName}' is disabled. (code: {AgentToolsErrorCodes.ToolDisabled})");
        }
    }

    [Theory]
    [MemberData(nameof(HostAndTenants))]
    public async Task Read_Tool_That_Requires_Approval_Is_Marked_And_Refused_Without_Approval_Support(Guid? tenantId)
    {
        using var _ = ChangeTenant(tenantId);

        (await ListAsync(WithApproval))["Book_Get"].RequiresApproval.ShouldBeTrue();
        (await ListAsync(WithoutApproval)).ShouldNotContainKey("Book_Get");
        (await InvokeAsync("Book_Get", WithoutApproval, new { id = Guid.NewGuid() })).ShouldContain(AgentToolsErrorCodes.ApprovalNotSupported);
    }

    [Theory]
    [MemberData(nameof(HostAndTenants))]
    public async Task Write_Tool_Without_Approval_Is_Listed_And_Runs_Without_Approval_Support(Guid? tenantId)
    {
        using var _ = ChangeTenant(tenantId);

        (await ListAsync(WithoutApproval))["Book_Update"].RequiresApproval.ShouldBeFalse();
        // It runs: the service itself does not find the book.
        (await InvokeAsync("Book_Update", WithoutApproval, new { id = Guid.NewGuid(), input = new { name = "New" } }))
            .ShouldStartWith("ERROR EntityNotFoundException");
    }

    [Fact]
    public async Task Fields_That_Are_Not_Set_And_Tools_Without_Settings_Keep_The_Code_Default()
    {
        var tools = await ListAsync(WithApproval);

        tools["Book_CreateThenFail"].RequiresApproval.ShouldBeTrue(); // only Enabled is set
        tools["Book_Archive"].RequiresApproval.ShouldBeFalse();       // {}: [AgentTool(RequiresApproval = false)]
        tools["Book_GetCount"].RequiresApproval.ShouldBeTrue();       // null: [AgentTool(RequiresApproval = true)]
    }

    [Fact]
    public async Task Code_Applies_And_The_Configuration_Overrides_It_Per_Field()
    {
        (await ListAsync(WithApproval)).ShouldNotContainKey("OtherSample_Register");
        (await InvokeAsync("OtherSample_Register", WithApproval)).ShouldContain(AgentToolsErrorCodes.ToolDisabled);

        var definition = GetRequiredService<IAgentToolCatalog>().Find("OtherSample_Register")!;
        (await GetRequiredService<IAgentToolPolicyStore>().GetAsync(definition)).ShouldBe(new EffectiveAgentToolPolicy(false, false));
    }

    private async Task<Dictionary<string, AgentTool>> ListAsync(AgentToolContext context)
    {
        return (await GetRequiredService<IAgentToolProvider>().GetToolsAsync(context)).ToDictionary(t => t.Definition.Name);
    }

    private Task<string> InvokeAsync(string toolName, AgentToolContext context, object? arguments = null)
    {
        var values = new AIFunctionArguments();
        if (arguments != null)
        {
            foreach (var property in JsonSerializer.SerializeToElement(arguments).EnumerateObject())
            {
                values[property.Name] = property.Value.Clone();
            }
        }

        return GetRequiredService<IAgentToolInvoker>().InvokeAsync(toolName, values, context);
    }
}

// Application start: the section is validated, names that are not in the catalogue are logged.
public class AgentToolPolicyStartupTests
{
    [Theory]
    [InlineData("""{"Book_GetList":{"Enabled":"flase"}}""", "AgentTools:Tools:Book_GetList:Enabled", true)]
    [InlineData("""{"Book_GetList":{"Enable":false}}""", "AgentTools:Tools:Book_GetList:Enable", true)]
    [InlineData("""{"Book_GetList":false}""", "AgentTools:Tools:Book_GetList", true)]
    [InlineData("false", "AgentTools:Tools", true)]
    [InlineData("""["Book_GetList"]""", "AgentTools:Tools:0", true)]
    // also when nothing is included
    [InlineData("""{"Book_GetList":{"Enabled":"flase"}}""", "AgentTools:Tools:Book_GetList:Enabled", false)]
    [InlineData("""{"Book_GetList":{"Enable":false}}""", "AgentTools:Tools:Book_GetList:Enable", false)]
    [InlineData("""{"Book_GetList":false}""", "AgentTools:Tools:Book_GetList", false)]
    public void Invalid_Section_Stops_The_Start_And_Names_The_Key(string tools, string key, bool include)
    {
        var exception = Should.Throw<Exception>(() => new PolicyHost(tools, include ? SampleRemoteServices.AgentToolsSample : null));

        var messages = "";
        for (var e = exception; e != null; e = e.InnerException)
        {
            messages += e.Message + "\n";
        }

        messages.ShouldContain($"'{key}'");
    }

    [Fact]
    public void Names_That_Are_Not_In_The_Catalogue_Give_One_Error_Log_And_The_Start_Continues()
    {
        using var host = new PolicyHost(
            """{"Book_GetLists":{"Enabled":false},"book_getlist":{"Enabled":false},"Product_Delete":{},"Book_ResetAll":null}""",
            SampleRemoteServices.AgentToolsSample);

        var entry = host.Logger.Entries.ShouldHaveSingleItem();
        entry.Level.ShouldBe(LogLevel.Error);
        entry.Message.ShouldContain("not in the catalogue");
        entry.Message.ShouldContain("no effect");
        entry.Message.ShouldContain("Book_GetLists, Book_ResetAll, Product_Delete"); // Book_ResetAll: [DisableAgentTool]
        entry.Message.ShouldNotContain("book_getlist", Case.Sensitive);
        host.Catalog.Find("Book_GetList").ShouldNotBeNull();
    }

    [Fact]
    public void Name_Set_In_Code_Is_Checked_Too()
    {
        using var host = new PolicyHost(null, SampleRemoteServices.AgentToolsSample,
            o => o.Tools["Product_Delete"] = new AgentToolPolicyConfiguration { Enabled = false });

        host.Logger.Entries.ShouldHaveSingleItem().Message.ShouldContain("Product_Delete");
    }

    [Fact]
    public void Tool_Left_Out_For_Its_Invalid_Schema_Is_Not_In_The_Catalogue()
    {
        using var host = new PolicyHost(
            "{\"" + DiagnosticsSampleAppService.CreateWithInvalidPattern + "\":{\"Enabled\":false},\""
            + DiagnosticsSampleAppService.UpdatePatch + "\":{\"Enabled\":false}}",
            DiagnosticsSampleAppService.RemoteServiceName);

        var message = host.Logger.Entries.ShouldHaveSingleItem().Message;
        message.ShouldContain(DiagnosticsSampleAppService.CreateWithInvalidPattern);
        message.ShouldNotContain(DiagnosticsSampleAppService.UpdatePatch);
    }

    [Fact]
    public void With_Nothing_Included_Every_Name_Is_Logged_And_The_Start_Continues()
    {
        using var host = new PolicyHost("""{"Book_GetList":{"Enabled":false},"Book_Get":{"Enabled":false}}""", null);

        var entry = host.Logger.Entries.ShouldHaveSingleItem();
        entry.Level.ShouldBe(LogLevel.Error);
        entry.Message.ShouldContain("Book_Get, Book_GetList");
        host.Catalog.Tools.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(null, SampleRemoteServices.AgentToolsSample)]
    [InlineData("{}", SampleRemoteServices.AgentToolsSample)]
    [InlineData("""{"BOOK_GETLIST":{"Enabled":false}}""", SampleRemoteServices.AgentToolsSample)]
    [InlineData(null, null)]
    public void Nothing_Is_Logged_Without_Unknown_Names(string? tools, string? include)
    {
        using var host = new PolicyHost(tools, include);

        host.Logger.Entries.ShouldBeEmpty();
    }

    // The "AgentTools" section as a whole: settings that look like a tool policy, but are not in AgentTools:Tools.
    [Theory]
    [InlineData("false", "AgentTools")]
    [InlineData("[]", "AgentTools")]
    [InlineData("""{"Tool":{"Book_GetList":{"Enabled":false}}}""", "AgentTools:Tool:Book_GetList:Enabled")]
    [InlineData("""{"Tool":{"Book_GetList":{"RequiresApproval":true}}}""", "AgentTools:Tool:Book_GetList:RequiresApproval")]
    [InlineData("""{"Book_GetList":{"Enabled":false}}""", "AgentTools:Book_GetList:Enabled")]
    [InlineData("""{"book_getlist":{"enabled":"flase"}}""", "AgentTools:book_getlist:enabled")]
    [InlineData("""{"Policies":{"Tools":{"Book_GetList":{"Enabled":false}}}}""", "AgentTools:Policies:Tools:Book_GetList:Enabled")]
    [InlineData("""[{"Enabled":false}]""", "AgentTools:0:Enabled")]
    [InlineData("""{"Tool":{"B":{"Enabled":false,"RequiresApproval":true}},"A":{"Enabled":true},"Tools":{"Book_GetList":{"Enabled":false}}}""",
        "AgentTools:A:Enabled, AgentTools:Tool:B:Enabled, AgentTools:Tool:B:RequiresApproval")]
    public void Policy_Settings_Outside_The_Section_Give_One_Error_Log_And_The_Start_Continues(string agentTools, string keys)
    {
        using var host = new PolicyHost(null, SampleRemoteServices.AgentToolsSample, json: "{\"AgentTools\":" + agentTools + "}");

        var entry = host.Logger.Entries.ShouldHaveSingleItem();
        entry.Level.ShouldBe(LogLevel.Error);
        entry.Message.ShouldEndWith(": " + keys);
        entry.Message.ShouldContain("AgentTools:Tools");
        host.Catalog.Find("Book_GetList").ShouldNotBeNull();
    }

    [Fact]
    public void Policy_Settings_Outside_The_Section_Are_Logged_When_Nothing_Is_Included()
    {
        using var host = new PolicyHost(null, null, json: """{"AgentTools":{"Tool":{"Book_GetList":{"Enabled":false}}}}""");

        host.Logger.Entries.ShouldHaveSingleItem().Message.ShouldEndWith(": AgentTools:Tool:Book_GetList:Enabled");
    }

    [Theory]
    [InlineData("""{"AgentTools":null}""")]
    [InlineData("""{"AgentTools":{"Enabled":false,"RequiresApproval":true}}""")] // not a tool; an application's own switch
    [InlineData("""{"AgentTools":{}}""")]
    [InlineData("""{"AgentTools":{"Tools":{"Book_GetList":{"Enabled":false,"RequiresApproval":true}}}}""")]
    [InlineData("""{"agenttools":{"tools":{"Book_GetList":{"enabled":false}}}}""")]
    [InlineData("""{"AgentTools":{"Mcp":{"Enabled":false,"Clients":{"c":{"DisplayName":"C","Enabled":true,"RequiresApproval":true}}}}}""")]
    [InlineData("""{"AgentTools":{"Other":{"Name":"x","Enabled":null,"Enable":true,"IsEnabled":true},"Enabled":{"Name":"x"}}}""")]
    [InlineData("""{"AgentTool":{"Tools":{"Book_GetList":{"Enabled":false}}},"AgentToolsDemo":{"Enabled":true}}""")]
    public void Other_Settings_Are_Not_Logged(string json)
    {
        using var host = new PolicyHost(null, SampleRemoteServices.AgentToolsSample, json: json);

        host.Logger.Entries.ShouldBeEmpty();
    }

    private sealed class PolicyHost(
        string? tools, string? include, Action<AbpAgentToolPolicyOptions>? configure = null, string? json = null)
        : AgentToolPolicyConfigurationTestBase
    {
        // Field initializers and captured primary-constructor parameters are set before the base constructor starts the host.
        public ListLogger<AbpAgentToolsAspNetCoreModule> Logger { get; } = new();

        public IAgentToolCatalog Catalog => GetRequiredService<IAgentToolCatalog>();

        protected override string? Tools => tools;

        protected override string? Json => json ?? base.Json;

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger<AbpAgentToolsAspNetCoreModule>>(Logger);
            if (include != null)
            {
                services.Configure<AbpAgentToolOptions>(o => o.IncludedRemoteServiceNames.Add(include));
            }

            if (configure != null)
            {
                services.Configure(configure);
            }
        }
    }
}
