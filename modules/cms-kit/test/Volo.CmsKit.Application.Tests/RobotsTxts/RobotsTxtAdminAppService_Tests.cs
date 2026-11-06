using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.RobotsTxts;
using Volo.CmsKit.Public.RobotsTxts;
using Xunit;

namespace Volo.CmsKit.RobotsTxts;

public class RobotsTxtAdminAppService_Tests : CmsKitApplicationTestBase
{
    private readonly IRobotsTxtAdminAppService _robotsTxtAdminAppService;
    private readonly IRobotsTxtPublicAppService _robotsTxtPublicAppService;
    private readonly ICurrentTenant _currentTenant;

    public RobotsTxtAdminAppService_Tests()
    {
        _robotsTxtAdminAppService = GetRequiredService<IRobotsTxtAdminAppService>();
        _robotsTxtPublicAppService = GetRequiredService<IRobotsTxtPublicAppService>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public async Task GetAsync_Should_Return_Default_When_Nothing_Is_Stored()
    {
        var robotsTxt = await _robotsTxtAdminAppService.GetAsync();

        robotsTxt.Source.ShouldBe(RobotsTxtSource.Default);
        robotsTxt.Content.ShouldBeNull();
        (await _robotsTxtPublicAppService.GetAsync()).Content.ShouldBeNull();
    }

    [Fact]
    public async Task GetAsync_Should_Return_The_Source_Of_The_Content()
    {
        await _robotsTxtAdminAppService.UpdateAsync(new UpdateRobotsTxtDto { Content = "User-agent: *\nDisallow: /host" });

        (await _robotsTxtAdminAppService.GetAsync()).Source.ShouldBe(RobotsTxtSource.Custom);

        using (_currentTenant.Change(Guid.NewGuid()))
        {
            var inherited = await _robotsTxtAdminAppService.GetAsync();
            inherited.Source.ShouldBe(RobotsTxtSource.InheritedFromHost);
            inherited.Content.ShouldBe("User-agent: *\nDisallow: /host");

            await _robotsTxtAdminAppService.UpdateAsync(new UpdateRobotsTxtDto { Content = "User-agent: *\nDisallow: /tenant" });

            var custom = await _robotsTxtAdminAppService.GetAsync();
            custom.Source.ShouldBe(RobotsTxtSource.Custom);
            custom.Content.ShouldBe("User-agent: *\nDisallow: /tenant");
            (await _robotsTxtPublicAppService.GetAsync()).Content.ShouldBe("User-agent: *\nDisallow: /tenant");

            await _robotsTxtAdminAppService.ResetAsync();

            (await _robotsTxtAdminAppService.GetAsync()).Source.ShouldBe(RobotsTxtSource.InheritedFromHost);
            (await _robotsTxtPublicAppService.GetAsync()).Content.ShouldBe("User-agent: *\nDisallow: /host");
        }
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Content_Is_Too_Long()
    {
        await Should.ThrowAsync<AbpValidationException>(async () =>
            await _robotsTxtAdminAppService.UpdateAsync(new UpdateRobotsTxtDto
            {
                Content = new string('a', RobotsTxtConsts.MaxContentLength + 1)
            }));
    }
}
