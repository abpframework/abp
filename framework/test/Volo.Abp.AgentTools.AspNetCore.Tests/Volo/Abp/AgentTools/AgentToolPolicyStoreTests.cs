using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.AgentTools.Sample;
using Xunit;

namespace Volo.Abp.AgentTools;

public class AgentToolPolicyStoreTests : AgentToolsAspNetCoreTestBase
{
    private readonly AgentToolDefinition _create;
    private readonly AgentToolDefinition _get;

    public AgentToolPolicyStoreTests()
    {
        var catalog = CreateCatalog(o => o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample));
        _create = catalog.Find("Book_Create")!; // approval by default
        _get = catalog.Find("Book_Get")!;       // no approval by default
    }

    [Fact]
    public async Task Configuration_Store_Is_Registered_And_Returns_The_Code_Defaults_Without_A_Section()
    {
        var store = GetRequiredService<IAgentToolPolicyStore>();
        store.ShouldBeOfType<ConfigurationAgentToolPolicyStore>();

        (await store.GetAsync(_create)).ShouldBe(new EffectiveAgentToolPolicy(true, true));
        (await store.GetAsync(_get)).ShouldBe(new EffectiveAgentToolPolicy(true, false));
    }
}
