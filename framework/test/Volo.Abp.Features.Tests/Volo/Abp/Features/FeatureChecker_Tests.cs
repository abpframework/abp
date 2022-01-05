using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.Features;

public class FeatureChecker_Tests : FeatureTestBase
{
    private readonly IFeatureChecker _featureChecker;
    private readonly ICurrentTenant _currentTenant;

    public FeatureChecker_Tests()
    {
        _featureChecker = GetRequiredService<IFeatureChecker>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public async Task IsEnabledAsync()
    {
        //Tenant is unknown
        (await _featureChecker.IsEnabledAsync("BooleanTestFeature1")).ShouldBeFalse();

        using (_currentTenant.Change(TestFeatureStore.Tenant1Id))
        {
            (await _featureChecker.IsEnabledAsync("BooleanTestFeature1")).ShouldBeTrue();
        }

        using (_currentTenant.Change(TestFeatureStore.Tenant2Id))
        {
            (await _featureChecker.IsEnabledAsync("BooleanTestFeature1")).ShouldBeFalse();
        }
    }

    [Fact]
    public async Task GetOrNullAsync()
    {
        //Tenant is unknown
        (await _featureChecker.GetOrNullAsync("IntegerTestFeature1")).ShouldBe("1");

        using (_currentTenant.Change(TestFeatureStore.Tenant1Id))
        {
            (await _featureChecker.GetOrNullAsync("IntegerTestFeature1")).ShouldBe("1");
        }

        using (_currentTenant.Change(TestFeatureStore.Tenant2Id))
        {
            (await _featureChecker.GetOrNullAsync("IntegerTestFeature1")).ShouldBe("34");
        }
    }
}
