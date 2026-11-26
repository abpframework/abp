using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.Features;

public class ExplicitInterfaceFeature_Tests : FeatureTestBase
{
    [Fact]
    public async Task Should_Require_The_Feature_For_An_Explicit_Interface_Method()
    {
        var service = GetRequiredService<IExplicitlyFeaturedService>();
        using (GetRequiredService<ICurrentTenant>().Change(null))
        {
            await Should.ThrowAsync<AbpAuthorizationException>(() => service.ReadAsync());
        }
    }

    [Fact]
    public async Task Should_Allow_The_Interface_Caller_When_The_Feature_Is_Enabled()
    {
        var service = GetRequiredService<IExplicitlyFeaturedService>();
        using (GetRequiredService<ICurrentTenant>().Change(TestFeatureStore.Tenant1Id))
        {
            (await service.ReadAsync()).ShouldBe(42);
        }
    }

    [Fact]
    public async Task Should_Keep_Protected_Helpers_Available_When_Feature_Checking_Is_Disabled()
    {
        var service = GetRequiredService<ExplicitlyFeaturedService>();
        using (GetRequiredService<ICurrentTenant>().Change(null))
        {
            (await service.UnrestrictedAsync()).ShouldBe(42);
        }
    }
}

public interface IExplicitlyFeaturedService
{
    Task<int> ReadAsync();
}

[RequiresFeature("BooleanTestFeature1")]
public class ExplicitlyFeaturedService : IExplicitlyFeaturedService, ITransientDependency
{
    Task<int> IExplicitlyFeaturedService.ReadAsync() => Task.FromResult(42);

    [DisableFeatureCheck]
    public virtual Task<int> UnrestrictedAsync() => ReadInternalAsync();

    protected virtual Task<int> ReadInternalAsync() => Task.FromResult(42);
}
