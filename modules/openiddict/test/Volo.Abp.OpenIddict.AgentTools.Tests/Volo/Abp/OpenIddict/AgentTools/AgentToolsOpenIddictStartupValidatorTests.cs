using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Server;
using Shouldly;
using Volo.Abp.Data;
using Xunit;

namespace Volo.Abp.OpenIddict.AgentTools;

public abstract class AgentToolsOpenIddictStartupValidatorTestBase : AgentToolsOpenIddictTestBase
{
    protected async Task<List<string>> ValidateAsync()
    {
        Logs.Clear();
        await WithUnitOfWorkAsync(sp => sp.GetRequiredService<AgentToolsOpenIddictStartupValidator>().ValidateAsync());
        return WarningsOf<AgentToolsOpenIddictStartupValidator>();
    }

    /// <summary>The errors of the last <see cref="ValidateAsync"/>.</summary>
    protected List<string> Errors => MessagesOf<AgentToolsOpenIddictStartupValidator>(LogLevel.Error);

    protected void ShouldHaveOneErrorFor(string storedResource)
    {
        var error = Errors.ShouldHaveSingleItem();
        error.ShouldContain("scope AgentTools ");
        error.ShouldContain($"[{storedResource}]");
        error.ShouldContain(McpUrl);
        error.ShouldContain("Update the scope's resources");
        error.ShouldContain("refresh tokens");
    }
}

public class AgentToolsOpenIddictStartupValidatorTests : AgentToolsOpenIddictStartupValidatorTestBase
{
    [Fact]
    public void Warns_At_Startup_When_The_Scope_Is_Missing()
    {
        WarningsOf<AgentToolsOpenIddictStartupValidator>().ShouldHaveSingleItem()
            .ShouldContain("OpenIddict scope AgentTools does not exist");
    }

    [Fact]
    public async Task No_Warning_When_The_Scope_And_The_Resource_Are_In_Place()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();

        (await ValidateAsync()).ShouldBeEmpty();
        Errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("https://old.test/mcp")] // App:SelfUrl changed after the scope was seeded
    [InlineData("https://app.test/MCP")] // audiences are compared with case
    [InlineData(AgentToolsOpenIddictTestModule.AppAudience)]
    [InlineData("")]
    public async Task Logs_One_Error_And_No_Warning_When_The_Scope_Does_Not_Have_The_Mcp_Url(string storedResource)
    {
        await CreateScopeAsync("AgentTools", storedResource.Length == 0 ? [] : [storedResource]);

        (await ValidateAsync()).ShouldBeEmpty();
        ShouldHaveOneErrorFor(storedResource);
    }

    [Fact]
    public async Task No_Error_When_The_Scope_Has_Other_Resources_Next_To_The_Mcp_Url()
    {
        await CreateScopeAsync("AgentTools", "Elsewhere", McpUrl);

        (await ValidateAsync()).ShouldBeEmpty();
        Errors.ShouldBeEmpty();
    }
}

/// <summary>An app that calls no AddAudiences: its OpenIddict validation accepts any audience.</summary>
public class AgentToolsOpenIddictStartupValidatorWithoutAudiencesTests : AgentToolsOpenIddictStartupValidatorTestBase
{
    protected override bool AcceptsAnyAudience => true;

    [Theory]
    [InlineData("https://old.test/mcp")]
    [InlineData("")]
    public async Task Logs_One_Error_And_No_Warning_When_The_Scope_Does_Not_Have_The_Mcp_Url(string storedResource)
    {
        await CreateScopeAsync("AgentTools", storedResource.Length == 0 ? [] : [storedResource]);

        (await ValidateAsync()).ShouldBeEmpty();
        ShouldHaveOneErrorFor(storedResource);
    }

    [Fact]
    public async Task No_Error_For_The_Seeded_Scope()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();

        (await ValidateAsync()).ShouldBeEmpty();
        Errors.ShouldBeEmpty();
    }
}

public class AgentToolsOpenIddictStartupValidatorWithScopeResourcesTests : AgentToolsOpenIddictStartupValidatorTestBase
{
    // Without the MCP URL, on purpose.
    protected override void ConfigureOptions(AbpAgentToolsOpenIddictOptions options) => options.ScopeResources.Add("Elsewhere");

    [Fact]
    public async Task Warns_When_The_Scope_Shares_No_Audience_With_The_Validation()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();

        (await ValidateAsync()).ShouldHaveSingleItem().ShouldContain("none of which is an audience");
        Errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task No_Error_When_The_Mcp_Url_Is_Not_One_Of_The_Configured_Resources()
    {
        await CreateScopeAsync("AgentTools", "Elsewhere", AgentToolsOpenIddictTestModule.AppAudience);

        (await ValidateAsync()).ShouldBeEmpty();
        Errors.ShouldBeEmpty();
    }
}

public class AgentToolsOpenIddictStartupValidatorWithoutMcpUrlTests : AgentToolsOpenIddictStartupValidatorTestBase
{
    protected override Dictionary<string, string?> Configuration => new();

    protected override void ConfigureOptions(AbpAgentToolsOpenIddictOptions options) => options.SyncMcpResourcePermission = false;

    [Fact]
    public void A_Web_Host_With_The_Resource_Permission_Sync_On_Does_Not_Start_Without_The_Mcp_Url()
    {
        var exception = Should.Throw<Exception>(() =>
        {
            using var host = new HostWithoutMcpUrl();
        });

        exception.ToString().ShouldContain(AgentToolsOpenIddictStartupValidator.McpResourceRequiredMessage);
    }

    [Fact]
    public async Task With_The_Sync_Off_Warns_When_The_Mcp_Url_Is_Unknown_And_The_Scope_Is_Missing()
    {
        var warnings = await ValidateAsync();

        warnings.Count.ShouldBe(2);
        warnings.ShouldContain(m => m.Contains("URL is unknown") && m.Contains("App:SelfUrl"));
        warnings.ShouldContain(m => m.Contains("OpenIddict scope AgentTools does not exist"));
    }

    /// <summary>No App:SelfUrl, no McpResource, default options (sync on).</summary>
    private sealed class HostWithoutMcpUrl : AgentToolsOpenIddictTestBase
    {
        protected override Dictionary<string, string?> Configuration => new();
    }
}

public class AgentToolsOpenIddictStartupValidatorWithUnregisteredResourceTests : AgentToolsOpenIddictStartupValidatorTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.PostConfigure<OpenIddictServerOptions>(options => options.Resources.Clear());
    }

    [Fact]
    public async Task Warns_When_The_Mcp_Url_Is_Not_A_Registered_Resource()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();

        (await ValidateAsync()).ShouldHaveSingleItem().ShouldContain($"MCP endpoint {McpUrl} is not a registered OpenIddict resource");
    }
}

public class AgentToolsOpenIddictStartupValidatorInADataMigrationTests : AgentToolsOpenIddictStartupValidatorTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddDataMigrationEnvironment();
    }

    [Fact]
    public void The_Startup_Check_Is_Skipped_In_A_Web_Host_Started_To_Migrate()
    {
        // The single-layer template's --migrate-database: a web host that initializes before it migrates and seeds.
        WarningsOf<AgentToolsOpenIddictStartupValidator>().ShouldBeEmpty();
    }
}

public class AgentToolsOpenIddictModuleOutsideAWebHostTests
{
    [Fact]
    public void The_Startup_Check_Is_Skipped_Without_A_Web_Pipeline()
    {
        // A DbMigrator or another console host: no application builder, so the startup validator is not resolved.
        using var services = new ServiceCollection().BuildServiceProvider();

        Should.NotThrow(() => new AbpOpenIddictAgentToolsModule().OnApplicationInitialization(new ApplicationInitializationContext(services)));
    }
}
