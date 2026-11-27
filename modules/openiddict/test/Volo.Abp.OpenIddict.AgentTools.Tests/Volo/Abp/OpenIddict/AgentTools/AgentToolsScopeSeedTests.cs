using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Xunit;

namespace Volo.Abp.OpenIddict.AgentTools;

public class AgentToolsScopeSeedTests : AgentToolsOpenIddictTestBase
{
    [Fact]
    public async Task Scope_Is_Created_With_English_And_Turkish_Display_Names_And_The_Mcp_Url_As_Its_Resource()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();

        var (displayName, displayNames, resources) = await ReadScopeAsync();
        displayName.ShouldBe(AgentToolsScopeDataSeedContributor.EnglishDisplayName);
        displayNames[CultureInfo.GetCultureInfo("en")].ShouldBe(AgentToolsScopeDataSeedContributor.EnglishDisplayName);
        displayNames[CultureInfo.GetCultureInfo("tr")].ShouldBe(AgentToolsScopeDataSeedContributor.TurkishDisplayName);
        resources.ShouldBe([McpUrl]);
    }

    [Fact]
    public async Task Seeding_Again_Keeps_One_Scope_And_Leaves_An_Edited_One_Unchanged()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();
        await WithUnitOfWorkAsync(async sp =>
        {
            var manager = sp.GetRequiredService<IOpenIddictScopeManager>();
            var scope = (await manager.FindByNameAsync("AgentTools"))!;
            var descriptor = new OpenIddictScopeDescriptor();
            await manager.PopulateAsync(descriptor, scope);
            descriptor.DisplayName = "Edited by an administrator";
            await manager.UpdateAsync(scope, descriptor);
        });

        await SeedAsync<AgentToolsScopeDataSeedContributor>();

        (await WithUnitOfWorkAsync(sp => sp.GetRequiredService<IOpenIddictScopeManager>().CountAsync().AsTask())).ShouldBe(1);
        (await ReadScopeAsync()).DisplayName.ShouldBe("Edited by an administrator");
    }

    private Task<(string? DisplayName, ImmutableDictionary<CultureInfo, string> DisplayNames, List<string> Resources)> ReadScopeAsync() =>
        WithUnitOfWorkAsync(async sp =>
        {
            var manager = sp.GetRequiredService<IOpenIddictScopeManager>();
            var scope = await manager.FindByNameAsync("AgentTools");
            scope.ShouldNotBeNull();
            return (await manager.GetDisplayNameAsync(scope), await manager.GetDisplayNamesAsync(scope),
                (await manager.GetResourcesAsync(scope)).ToList());
        });
}

public class AgentToolsScopeSeedWithConfiguredResourcesTests : AgentToolsOpenIddictTestBase
{
    protected override void ConfigureOptions(AbpAgentToolsOpenIddictOptions options)
    {
        options.ScopeName = "Agents";
        options.ScopeResources.Add(AgentToolsOpenIddictTestModule.AppAudience);
    }

    [Fact]
    public async Task Configured_Scope_Name_And_Resources_Replace_The_Defaults()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();

        var resources = await WithUnitOfWorkAsync(async sp =>
        {
            var manager = sp.GetRequiredService<IOpenIddictScopeManager>();
            (await manager.FindByNameAsync("AgentTools")).ShouldBeNull();
            return (await manager.GetResourcesAsync((await manager.FindByNameAsync("Agents"))!)).ToList();
        });
        resources.ShouldBe([AgentToolsOpenIddictTestModule.AppAudience]);
    }
}

public class AgentToolsScopeSeedWithoutMcpUrlTests : AgentToolsOpenIddictTestBase
{
    protected override Dictionary<string, string?> Configuration => new();

    // Otherwise the host does not start without the MCP URL (AgentToolsOpenIddictStartupValidator).
    protected override void ConfigureOptions(AbpAgentToolsOpenIddictOptions options) => options.SyncMcpResourcePermission = false;

    [Fact]
    public async Task Scope_Without_Any_Resource_Is_Not_Created_And_That_Is_Reported()
    {
        await SeedAsync<AgentToolsScopeDataSeedContributor>();

        (await WithUnitOfWorkAsync(sp => sp.GetRequiredService<IOpenIddictScopeManager>().FindByNameAsync("AgentTools").AsTask())).ShouldBeNull();
        WarningsOf<AgentToolsScopeDataSeedContributor>().ShouldHaveSingleItem().ShouldContain("App:SelfUrl");
    }
}
