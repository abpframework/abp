using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.CmsKit.RobotsTxts;

public abstract class RobotsTxtManager_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly RobotsTxtManager _robotsTxtManager;
    private readonly ICurrentTenant _currentTenant;

    protected RobotsTxtManager_Test()
    {
        _robotsTxtManager = GetRequiredService<RobotsTxtManager>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public async Task GetEffectiveContentOrNullAsync_Should_Return_Null_When_Nothing_Is_Stored()
    {
        using (_currentTenant.Change(Guid.NewGuid()))
        {
            var content = await WithUnitOfWorkAsync(() => _robotsTxtManager.GetEffectiveContentOrNullAsync());

            content.ShouldBeNull();
        }
    }

    [Fact]
    public async Task Tenant_Should_Inherit_Host_Content_Until_It_Overrides_And_Again_After_Reset()
    {
        const string hostContent = "User-agent: *\nDisallow: /Host";
        const string tenantContent = "User-agent: *\nDisallow: /Tenant";
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(null))
        {
            await WithUnitOfWorkAsync(() => _robotsTxtManager.SetAsync(hostContent));
        }

        using (_currentTenant.Change(tenantId))
        {
            (await WithUnitOfWorkAsync(() => _robotsTxtManager.GetEffectiveContentOrNullAsync())).ShouldBe(hostContent);

            await WithUnitOfWorkAsync(() => _robotsTxtManager.SetAsync(tenantContent));
            (await WithUnitOfWorkAsync(() => _robotsTxtManager.GetEffectiveContentOrNullAsync())).ShouldBe(tenantContent);

            await WithUnitOfWorkAsync(() => _robotsTxtManager.ResetAsync());
            (await WithUnitOfWorkAsync(() => _robotsTxtManager.GetEffectiveContentOrNullAsync())).ShouldBe(hostContent);
        }

        using (_currentTenant.Change(null))
        {
            (await WithUnitOfWorkAsync(() => _robotsTxtManager.GetEffectiveContentOrNullAsync())).ShouldBe(hostContent);

            await WithUnitOfWorkAsync(() => _robotsTxtManager.ResetAsync());
            (await WithUnitOfWorkAsync(() => _robotsTxtManager.GetEffectiveContentOrNullAsync())).ShouldBeNull();
        }
    }

    [Fact]
    public async Task Tenant_Override_Should_Be_Isolated_From_Other_Tenants()
    {
        var tenant1 = Guid.NewGuid();
        var tenant2 = Guid.NewGuid();

        using (_currentTenant.Change(tenant1))
        {
            await WithUnitOfWorkAsync(() => _robotsTxtManager.SetAsync("User-agent: *\nDisallow: /"));
        }

        using (_currentTenant.Change(tenant2))
        {
            (await WithUnitOfWorkAsync(() => _robotsTxtManager.FindAsync())).ShouldBeNull();
        }
    }

    [Fact]
    public async Task SetAsync_Should_Refresh_The_Cached_Content()
    {
        using (_currentTenant.Change(Guid.NewGuid()))
        {
            await WithUnitOfWorkAsync(() => _robotsTxtManager.SetAsync("User-agent: *\nDisallow: /a"));
            (await WithUnitOfWorkAsync(() => _robotsTxtManager.GetEffectiveContentOrNullAsync())).ShouldBe("User-agent: *\nDisallow: /a");

            await WithUnitOfWorkAsync(() => _robotsTxtManager.SetAsync("User-agent: *\nDisallow: /b"));
            (await WithUnitOfWorkAsync(() => _robotsTxtManager.GetEffectiveContentOrNullAsync())).ShouldBe("User-agent: *\nDisallow: /b");
        }
    }

    [Fact]
    public async Task SetAsync_Should_Keep_Case_And_Normalize_Line_Endings()
    {
        using (_currentTenant.Change(Guid.NewGuid()))
        {
            await WithUnitOfWorkAsync(() => _robotsTxtManager.SetAsync("User-agent: *\r\nDisallow: /Admin\rDisallow: /admin"));

            var robotsTxt = await WithUnitOfWorkAsync(() => _robotsTxtManager.FindAsync());

            robotsTxt.Content.ShouldBe("User-agent: *\nDisallow: /Admin\nDisallow: /admin");
        }
    }
}
