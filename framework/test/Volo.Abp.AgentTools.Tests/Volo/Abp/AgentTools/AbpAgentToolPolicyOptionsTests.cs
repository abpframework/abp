using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.AgentTools;

// The "AgentTools:Tools" section through the module, the options and the default store; no ASP.NET Core, so no startup check.
public class AbpAgentToolPolicyOptionsTests
{
    private static readonly AgentToolDefinition Delete = Tool("Product_Delete", requiresApprovalByDefault: true);
    private static readonly AgentToolDefinition Get = Tool("Product_Get", requiresApprovalByDefault: false);

    [Fact]
    public async Task No_Section_Gives_The_Code_Defaults()
    {
        using var app = Create(new ConfigurationBuilder().Build());
        var store = GetStore(app);

        GetOptions(app).Tools.ShouldBeEmpty();
        store.ShouldBeOfType<ConfigurationAgentToolPolicyStore>();
        (await store.GetAsync(Delete)).ShouldBe(new EffectiveAgentToolPolicy(true, true));
        (await store.GetAsync(Get)).ShouldBe(new EffectiveAgentToolPolicy(true, false));
    }

    [Theory]
    [InlineData("""{"Product_Delete":{"Enabled":false}}""", false, true)] // one field set: the other keeps its code default
    [InlineData("""{"Product_Delete":{"RequiresApproval":false}}""", true, false)]
    [InlineData("""{"Product_Delete":{"Enabled":false,"RequiresApproval":false}}""", false, false)]
    [InlineData("""{"Product_Delete":{"Enabled":"False","RequiresApproval":" FALSE "}}""", false, false)] // text
    [InlineData("""{"product_DELETE":{"enabled":false,"REQUIRESAPPROVAL":false}}""", false, false)] // any case
    [InlineData("""{"Product_Delete:Enabled":false}""", false, true)] // flat key
    [InlineData("""{"Product_Get":{"Enabled":false}}""", true, true)] // another tool
    [InlineData("""{"Product_Delete":{}}""", true, true)]
    [InlineData("""{"Product_Delete":null}""", true, true)]
    [InlineData("""{"Product_Delete":{"Enabled":null,"RequiresApproval":null}}""", true, true)]
    [InlineData("{}", true, true)]
    [InlineData("null", true, true)]
    public async Task Configured_Fields_Apply_And_The_Others_Keep_The_Code_Default(string tools, bool enabled, bool requiresApproval)
    {
        using var app = Create(Json(tools));

        (await GetStore(app).GetAsync(Delete)).ShouldBe(new EffectiveAgentToolPolicy(enabled, requiresApproval));
        (await GetStore(app).GetListAsync([Delete]))[Delete.Name].ShouldBe(new EffectiveAgentToolPolicy(enabled, requiresApproval));
    }

    [Fact]
    public async Task RequiresApproval_True_Applies_To_A_Read_Tool()
    {
        using var app = Create(Json("""{"Product_Get":{"RequiresApproval":true}}"""));

        (await GetStore(app).GetAsync(Get)).ShouldBe(new EffectiveAgentToolPolicy(true, true));
    }

    [Theory]
    // not a bool
    [InlineData("""{"Product_Delete":{"Enabled":"flase"}}""", "AgentTools:Tools:Product_Delete:Enabled", "true or false")]
    [InlineData("""{"Product_Delete":{"Enabled":""}}""", "AgentTools:Tools:Product_Delete:Enabled", "true or false")]
    [InlineData("""{"Product_Delete":{"Enabled":" "}}""", "AgentTools:Tools:Product_Delete:Enabled", "true or false")]
    [InlineData("""{"Product_Delete":{"Enabled":0}}""", "AgentTools:Tools:Product_Delete:Enabled", "true or false")]
    [InlineData("""{"Product_Delete":{"Enabled":"0"}}""", "AgentTools:Tools:Product_Delete:Enabled", "true or false")]
    [InlineData("""{"Product_Delete":{"RequiresApproval":"yes"}}""", "AgentTools:Tools:Product_Delete:RequiresApproval", "true or false")]
    [InlineData("""{"Product_Delete":{"Enabled":[]}}""", "AgentTools:Tools:Product_Delete:Enabled", "true or false")]
    [InlineData("""{"Product_Delete":{"Enabled":[false]}}""", "AgentTools:Tools:Product_Delete:Enabled", "true or false")]
    [InlineData("""{"Product_Delete":{"Enabled":{"Value":false}}}""", "AgentTools:Tools:Product_Delete:Enabled", "true or false")]
    // unknown field
    [InlineData("""{"Product_Delete":{"Enable":false}}""", "AgentTools:Tools:Product_Delete:Enable", "Enabled and RequiresApproval")]
    [InlineData("""{"Product_Delete":{"Enable":null}}""", "AgentTools:Tools:Product_Delete:Enable", "Enabled and RequiresApproval")]
    [InlineData("""{"Product_Delete":{"Enabled":false,"Roles":{"admin":true}}}""", "AgentTools:Tools:Product_Delete:Roles", "Enabled and RequiresApproval")]
    [InlineData("""{"Product_Delete":{"":false}}""", "AgentTools:Tools:Product_Delete:", "Enabled and RequiresApproval")]
    // not an object for the tool
    [InlineData("""{"Product_Delete":false}""", "AgentTools:Tools:Product_Delete", "an object")]
    [InlineData("""{"Product_Delete":""}""", "AgentTools:Tools:Product_Delete", "an object")]
    [InlineData("""{"Product_Delete":"x"}""", "AgentTools:Tools:Product_Delete", "an object")]
    [InlineData("""{"Product_Delete":[]}""", "AgentTools:Tools:Product_Delete", "an object")]
    [InlineData("""{"Product_Delete":["Enabled"]}""", "AgentTools:Tools:Product_Delete:0", "Enabled and RequiresApproval")]
    // not an object for the section
    [InlineData("false", "AgentTools:Tools", "an object")]
    [InlineData("\"\"", "AgentTools:Tools", "an object")]
    [InlineData("\"Product_Delete\"", "AgentTools:Tools", "an object")]
    [InlineData("[]", "AgentTools:Tools", "an object")]
    [InlineData("""["Product_Delete"]""", "AgentTools:Tools:0", "tool name")]
    [InlineData("""[{"Enabled":false}]""", "AgentTools:Tools:0", "tool name")]
    [InlineData("""{"":{"Enabled":false}}""", "AgentTools:Tools:", "tool name")]
    public async Task Anything_Else_Throws_On_Every_Use_And_Names_The_Key(string tools, string key, string expected)
    {
        using var app = Create(Json(tools));

        // Never the defaults, also not after the first failure.
        for (var i = 0; i < 2; i++)
        {
            ShouldNameTheKey(Should.Throw<AbpException>(() => GetOptions(app)), key, expected);
            ShouldNameTheKey(await Should.ThrowAsync<AbpException>(async () => await GetStore(app).GetAsync(Get)), key, expected);
            ShouldNameTheKey(await Should.ThrowAsync<AbpException>(async () => await GetStore(app).GetListAsync([Get])), key, expected);
        }
    }

    [Theory]
    [InlineData("""{"Product_Delete":{"Enabled":false}}""", """{"Product_Delete":true}""", "AgentTools:Tools:Product_Delete")]
    [InlineData("""{"Product_Delete":true}""", """{"Product_Delete":{"Enabled":false}}""", "AgentTools:Tools:Product_Delete")]
    [InlineData("""{"Product_Delete":{"Enabled":false}}""", "true", "AgentTools:Tools")]
    [InlineData("""{"Product_Delete":{"Enabled":{"Value":false}}}""", """{"Product_Delete":{"Enabled":false}}""", "AgentTools:Tools:Product_Delete:Enabled")]
    public void Scalar_From_One_Provider_And_Children_From_Another_Throw(string first, string second, string key)
    {
        using var app = Create(Json(first, second));

        ShouldNameTheKey(Should.Throw<AbpException>(() => GetOptions(app)), key, "");
    }

    [Fact]
    public void Later_Provider_Overrides_Per_Field()
    {
        using var app = Create(Json("""{"Product_Delete":{"Enabled":false,"RequiresApproval":false}}""", """{"product_delete":{"enabled":true}}"""));

        GetOptions(app).GetPolicy(Delete).ShouldBe(new EffectiveAgentToolPolicy(true, false));
    }

    [Fact]
    public void Environment_Variables_Apply_And_Are_Read_As_Strictly()
    {
        // Process wide, so a tool name of its own: other tests run at the same time.
        const string variable = "AgentTools__Tools__EnvironmentSample_Delete";
        var tool = Tool("EnvironmentSample_Delete", requiresApprovalByDefault: true);

        Read(variable + "__Enabled", "false").GetPolicy(tool).ShouldBe(new EffectiveAgentToolPolicy(false, false));
        Read(variable.ToUpperInvariant() + "__REQUIRESAPPROVAL", "TRUE").GetPolicy(tool).ShouldBe(new EffectiveAgentToolPolicy(true, true));
        ShouldNameTheKey(Should.Throw<AbpException>(() => Read(variable + "__Enabled", "0")), "AgentTools:Tools:EnvironmentSample_Delete:Enabled", "true or false");
        ShouldNameTheKey(Should.Throw<AbpException>(() => Read(variable + "__Enabled", "")), "AgentTools:Tools:EnvironmentSample_Delete:Enabled", "true or false");
        ShouldNameTheKey(Should.Throw<AbpException>(() => Read(variable, "false")), "AgentTools:Tools:EnvironmentSample_Delete", "an object");
        ShouldNameTheKey(Should.Throw<AbpException>(() => Read(variable, "")), "AgentTools:Tools:EnvironmentSample_Delete", "an object");

        // The variable over a file that says otherwise, in the usual order of the providers.
        static AbpAgentToolPolicyOptions Read(string name, string value)
        {
            Environment.SetEnvironmentVariable(name, value);
            try
            {
                using var app = Create(new ConfigurationBuilder()
                    .AddJsonStream(Stream("""{"AgentTools":{"Tools":{"EnvironmentSample_Delete":{"Enabled":true,"RequiresApproval":false}}}}"""))
                    .AddEnvironmentVariables()
                    .Build());
                return GetOptions(app);
            }
            finally
            {
                Environment.SetEnvironmentVariable(name, null);
            }
        }
    }

    [Fact]
    public void Code_Applies_And_The_Configuration_Overrides_It_Per_Field()
    {
        using var app = Create(
            Json("""{"product_delete":{"RequiresApproval":false},"Product_Get":{"Enabled":true}}"""),
            options =>
            {
                options.Tools["Product_Delete"] = new AgentToolPolicyConfiguration { Enabled = false, RequiresApproval = true };
                options.Tools["PRODUCT_GET"] = new AgentToolPolicyConfiguration { Enabled = false, RequiresApproval = true };
                options.Tools["Product_Update"] = new AgentToolPolicyConfiguration { Enabled = false };
            });
        var options = GetOptions(app);

        options.Tools.Count.ShouldBe(3);
        options.GetPolicy(Delete).ShouldBe(new EffectiveAgentToolPolicy(false, false));
        options.GetPolicy(Get).ShouldBe(new EffectiveAgentToolPolicy(true, true));
        options.GetPolicy(Tool("product_update", requiresApprovalByDefault: true)).ShouldBe(new EffectiveAgentToolPolicy(false, true));
    }

    [Fact]
    public void Configuration_Of_One_Tool_Does_Not_Change_Another_That_Code_Gave_The_Same_Instance()
    {
        var disabled = new AgentToolPolicyConfiguration { Enabled = false };
        using var app = Create(
            Json("""{"Product_Get":{"Enabled":true}}"""),
            options => options.Tools["Product_Get"] = options.Tools["Product_Delete"] = disabled);

        GetOptions(app).GetPolicy(Get).Enabled.ShouldBeTrue();
        GetOptions(app).GetPolicy(Delete).Enabled.ShouldBeFalse();
    }

    [Fact]
    public void Null_Entry_From_Code_Is_Not_Set()
    {
        using var app = Create(
            Json("""{"Product_Get":{"Enabled":false}}"""),
            options => options.Tools["Product_Get"] = options.Tools["Product_Delete"] = null!);

        GetOptions(app).GetPolicy(Delete).ShouldBe(new EffectiveAgentToolPolicy(true, true));
        GetOptions(app).GetPolicy(Get).ShouldBe(new EffectiveAgentToolPolicy(false, false));
    }

    [Fact]
    public async Task Host_And_Every_Tenant_Have_The_Same_Policy()
    {
        using var app = Create(Json("""{"Product_Delete":{"Enabled":false,"RequiresApproval":false}}"""));

        foreach (var tenantId in new Guid?[] { null, Guid.NewGuid(), Guid.NewGuid() })
        {
            using (app.ServiceProvider.GetRequiredService<ICurrentTenant>().Change(tenantId))
            {
                (await GetStore(app).GetAsync(Delete)).ShouldBe(new EffectiveAgentToolPolicy(false, false));
                var policies = await GetStore(app).GetListAsync([Delete, Get]);
                policies[Delete.Name].ShouldBe(new EffectiveAgentToolPolicy(false, false));
                policies[Get.Name].ShouldBe(new EffectiveAgentToolPolicy(true, false));
            }
        }
    }

    [Fact]
    public void The_Section_Is_Read_Once()
    {
        var configuration = Json("""{"Product_Delete":{"Enabled":false}}""");
        using var app = Create(configuration);
        GetOptions(app).GetPolicy(Delete).Enabled.ShouldBeFalse();

        configuration["AgentTools:Tools:Product_Delete:Enabled"] = "true";

        GetOptions(app).GetPolicy(Delete).Enabled.ShouldBeFalse();
    }

    [Fact]
    public void Store_Registered_By_Another_Module_Is_Kept()
    {
        using var app = AbpApplicationFactory.Create<PolicyTestModule>(options =>
            options.Services.AddSingleton<IAgentToolPolicyStore, InMemoryAgentToolPolicyStore>());
        app.Initialize();

        GetStore(app).ShouldBeOfType<InMemoryAgentToolPolicyStore>();
    }

    private static void ShouldNameTheKey(Exception exception, string key, string expected)
    {
        exception.Message.ShouldContain($"'{key}'");
        exception.Message.ShouldContain(expected);
    }

    private static AbpAgentToolPolicyOptions GetOptions(IAbpApplication app)
    {
        return app.ServiceProvider.GetRequiredService<IOptions<AbpAgentToolPolicyOptions>>().Value;
    }

    private static IAgentToolPolicyStore GetStore(IAbpApplication app)
    {
        return app.ServiceProvider.GetRequiredService<IAgentToolPolicyStore>();
    }

    private static IAbpApplicationWithInternalServiceProvider Create(IConfiguration configuration, Action<AbpAgentToolPolicyOptions>? configure = null)
    {
        var app = AbpApplicationFactory.Create<PolicyTestModule>(options =>
        {
            options.Services.ReplaceConfiguration(configuration);
            if (configure != null)
            {
                options.Services.Configure(configure);
            }
        });
        app.Initialize();
        return app;
    }

    /// <summary>One provider per given value of the section.</summary>
    private static IConfigurationRoot Json(params string[] tools)
    {
        var builder = new ConfigurationBuilder();
        foreach (var value in tools)
        {
            builder.AddJsonStream(Stream("{\"AgentTools\":{\"Tools\":" + value + "}}"));
        }

        return builder.Build();
    }

    private static MemoryStream Stream(string json) => new(Encoding.UTF8.GetBytes(json));

    private static AgentToolDefinition Tool(string name, bool requiresApprovalByDefault)
    {
        var method = typeof(object).GetMethod(nameof(ToString))!;
        return new AgentToolDefinition
        {
            Name = name,
            Description = name,
            Kind = AgentToolKind.Write,
            ReadOnly = !requiresApprovalByDefault,
            Destructive = requiresApprovalByDefault,
            Idempotent = false,
            RequiresApprovalByDefault = requiresApprovalByDefault,
            ServiceInterface = typeof(object),
            InterfaceMethod = method,
            ImplementationType = typeof(object),
            ImplementationMethod = method,
            HttpMethod = "POST",
            Url = "api/" + name,
            RemoteServiceName = "Test",
            ModuleRootPath = "test",
            ControllerName = "Test",
            ActionName = name,
            Parameters = []
        };
    }

    [DependsOn(typeof(AbpAgentToolsModule))]
    public class PolicyTestModule : AbpModule
    {
    }
}
