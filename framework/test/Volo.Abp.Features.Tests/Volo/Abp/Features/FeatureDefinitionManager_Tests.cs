using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Features;

public class FeatureDefinitionManager_Tests : FeatureTestBase
{
    private readonly IFeatureDefinitionManager _featureDefinitionManager;

    public FeatureDefinitionManager_Tests()
    {
        _featureDefinitionManager = GetRequiredService<IFeatureDefinitionManager>();
    }

    [Fact]
    public async Task Should_Get_Defined_Features()
    {
        await _featureDefinitionManager.GetOrNullAsync("BooleanTestFeature1").ShouldNotBeNull();
        (await _featureDefinitionManager.GetAsync("BooleanTestFeature1")).Name.ShouldBe("BooleanTestFeature1");

        await _featureDefinitionManager.GetOrNullAsync("IntegerTestFeature1").ShouldNotBeNull();
        (await _featureDefinitionManager.GetAsync("IntegerTestFeature1")).Name.ShouldBe("IntegerTestFeature1");
    }

    [Fact]
    public async Task Should_Not_Get_Undefined_Features()
    {
        (await _featureDefinitionManager.GetOrNullAsync("UndefinedFeature")).ShouldBeNull();
        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await _featureDefinitionManager.GetAsync("UndefinedFeature");
        });
    }
}
