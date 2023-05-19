using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.Features;

public class FeatureInterceptor_Tests : FeatureTestBase
{
    private readonly ClassFeatureTestService _classFeatureTestService;
    private readonly IMethodFeatureTestService _methodFeatureTestService;
    private readonly ICurrentTenant _currentTenant;

    public FeatureInterceptor_Tests()
    {
        _classFeatureTestService = GetRequiredService<ClassFeatureTestService>();
        _methodFeatureTestService = GetRequiredService<IMethodFeatureTestService>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Theory]
    [InlineData(null)] //Features were not enabled for null tenantid
    [InlineData(TestFeatureStore.Tenant2IdValue)] //Features were not enabled for Tenant 2
    public async Task Should_Not_Allow_To_Method_Calls_If_Related_Features_Were_Not_Enabled(string tenantIdValue)
    {
        using (_currentTenant.Change(ParseNullableGuid(tenantIdValue)))
        {
            await Assert.ThrowsAsync<AbpAuthorizationException>(async () =>
            {
                await _classFeatureTestService.NoAdditionalFeatureAsync();
            });

            await Assert.ThrowsAsync<AbpAuthorizationException>(async () =>
            {
                await _classFeatureTestService.Feature2Async();
            });

            await Assert.ThrowsAsync<AbpAuthorizationException>(async () =>
            {
                await _methodFeatureTestService.Feature1Async();
            });
        }
    }

    [Fact]
    public async Task Should_Allow_To_Method_Calls_If_Related_Features_Were_Enabled()
    {
        //Features were enabled for Tenant 1
        using (_currentTenant.Change(TestFeatureStore.Tenant1Id))
        {
            await _classFeatureTestService.NoAdditionalFeatureAsync();
            (await _classFeatureTestService.Feature2Async()).ShouldBe(42);
            (await _methodFeatureTestService.Feature1Async()).ShouldBe(42);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData(TestFeatureStore.Tenant1IdValue)]
    [InlineData(TestFeatureStore.Tenant2IdValue)]
    public async Task Should_Allow_To_Method_Calls_For_Those_Have_No_RequiresFeature_Attributes(string tenantIdValue)
    {
        using (_currentTenant.Change(ParseNullableGuid(tenantIdValue)))
        {
            await _methodFeatureTestService.NonFeatureAsync();
        }
    }

    private static Guid? ParseNullableGuid(string tenantIdValue)
    {
        return tenantIdValue == null ? (Guid?)null : new Guid(tenantIdValue);
    }
}
