using System;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Validation;
using Shouldly;
using Volo.Abp.AgentTools.Mcp;
using Xunit;

namespace Volo.Abp.OpenIddict.AgentTools;

/// <summary>The defaults, read from the resolved OpenIddict options of a real host.</summary>
public class AgentToolsOpenIddictDefaultOptionsTests : AgentToolsOpenIddictTestBase
{
    [Fact]
    public void Names_That_Applications_And_Clients_Use_Do_Not_Change()
    {
        AbpAgentToolsOpenIddictOptions.ClientsConfigurationSection.ShouldBe("AgentTools:Mcp:Clients");
        new AbpAgentToolsOpenIddictOptions().ScopeName.ShouldBe("AgentTools");
        new AbpAgentToolsMcpOptions().RequiredScope.ShouldBe("AgentTools");
    }

    [Fact]
    public void Plain_Pkce_Stays_Allowed_Unless_S256_Only_Is_Opted_In()
    {
        GetRequiredService<IOptions<OpenIddictServerOptions>>().Value.CodeChallengeMethods
            .ShouldBe([OpenIddictConstants.CodeChallengeMethods.Plain, OpenIddictConstants.CodeChallengeMethods.Sha256], ignoreOrder: true);
    }

    [Fact]
    public void The_Mcp_Url_From_App_SelfUrl_Is_A_Registered_Resource_And_An_Accepted_Audience()
    {
        GetRequiredService<IOptions<AbpAgentToolsOpenIddictOptions>>().Value.McpResource.ShouldBe(McpUrl);
        GetRequiredService<IOptions<OpenIddictServerOptions>>().Value.Resources.ShouldContain(new Uri(McpUrl));
        GetRequiredService<IOptions<OpenIddictValidationOptions>>().Value.Audiences
            .ShouldBe([AgentToolsOpenIddictTestModule.AppAudience, McpUrl], ignoreOrder: true);
    }

    [Fact]
    public void Token_Entry_Validation_Is_On_By_Default()
    {
        GetRequiredService<IOptions<OpenIddictValidationOptions>>().Value.EnableTokenEntryValidation.ShouldBeTrue();
    }
}

/// <summary>S256-only opted in, token entry validation turned off, an explicit MCP URL.</summary>
public class AgentToolsOpenIddictOptInOptionsTests : AgentToolsOpenIddictTestBase
{
    private const string ExplicitMcpUrl = "https://public.test/agents/mcp";

    protected override void ConfigureOptions(AbpAgentToolsOpenIddictOptions options)
    {
        options.RequireS256CodeChallengeMethod = true;
        options.EnableTokenEntryValidation = false;
        options.McpResource = ExplicitMcpUrl;
    }

    [Fact]
    public async Task With_The_Opt_In_Only_S256_Is_Allowed_And_Advertised()
    {
        GetRequiredService<IOptions<OpenIddictServerOptions>>().Value.CodeChallengeMethods
            .ShouldBe([OpenIddictConstants.CodeChallengeMethods.Sha256]);

        var discovery = await Client.GetFromJsonAsync<JsonElement>("/.well-known/openid-configuration");
        discovery.GetProperty("code_challenge_methods_supported").EnumerateArray().Select(e => e.GetString())
            .ShouldBe([OpenIddictConstants.CodeChallengeMethods.Sha256]);
    }

    [Fact]
    public void Token_Entry_Validation_Stays_Off_When_Disabled()
    {
        GetRequiredService<IOptions<OpenIddictValidationOptions>>().Value.EnableTokenEntryValidation.ShouldBeFalse();
    }

    [Fact]
    public void An_Explicit_Mcp_Url_Wins_Over_App_SelfUrl()
    {
        var resources = GetRequiredService<IOptions<OpenIddictServerOptions>>().Value.Resources;
        resources.ShouldContain(new Uri(ExplicitMcpUrl));
        resources.ShouldNotContain(new Uri(McpUrl));
        GetRequiredService<IOptions<OpenIddictValidationOptions>>().Value.Audiences.ShouldContain(ExplicitMcpUrl);
    }
}
