using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity.Integration;

public class IdentityUserIntegrationService_Tests : AbpIdentityApplicationTestBase
{
    private readonly IIdentityUserIntegrationService _identityUserIntegrationService;
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly ILookupNormalizer _lookupNormalizer;

    public IdentityUserIntegrationService_Tests()
    {
        _identityUserIntegrationService = GetRequiredService<IIdentityUserIntegrationService>();
        _identityUserRepository = GetRequiredService<IIdentityUserRepository>();
        _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
    }
    
    [Fact]
    public async Task GetRoleNamesAsync()
    {
        var adminUser = await _identityUserRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("admin"));
        adminUser.ShouldNotBeNull();
        
        var roles = await _identityUserIntegrationService.GetRoleNamesAsync(adminUser.Id);
        roles.Length.ShouldBe(1);
        roles.ShouldContain("admin");
    }

    [Fact]
    public async Task FindByIdAsync()
    {
        var user = await _identityUserRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserIntegrationService.FindByIdAsync(user.Id)).UserName.ShouldBe(user.UserName);
    }

    [Fact]
    public async Task FindById_NotExist_Should_Return_Null()
    {
        var user = await _identityUserIntegrationService.FindByIdAsync(Guid.NewGuid());
        user.ShouldBeNull();
    }

    [Fact]
    public async Task FindByUserNameAsync()
    {
        var user = await _identityUserRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserIntegrationService.FindByUserNameAsync(user.UserName)).UserName.ShouldBe(user.UserName);
    }

    [Fact]
    public async Task FindByUserName_NotExist_Should_Return_Null()
    {
        var user = await _identityUserIntegrationService.FindByUserNameAsync(Guid.NewGuid().ToString());
        user.ShouldBeNull();
    }

    [Fact]
    public async Task Search_Without_Filter_And_Sorting()
    {
        var result = await _identityUserIntegrationService.SearchAsync(new UserLookupSearchInputDto());
        result.Items.Count.ShouldBeGreaterThanOrEqualTo(3);
        result.Items.ShouldContain(u => u.UserName == "john.nash");
    }

    [Fact]
    public async Task Search_With_Filter()
    {
        var result = await _identityUserIntegrationService.SearchAsync(
            new UserLookupSearchInputDto
            {
                Filter = "a"
            }
        );

        result.Items.Count.ShouldBeGreaterThanOrEqualTo(2);
        result.Items.ShouldContain(u => u.UserName == "john.nash");
        result.Items.ShouldContain(u => u.UserName == "david");

        result = await _identityUserIntegrationService.SearchAsync(
            new UserLookupSearchInputDto
            {
                Filter = "neo"
            }
        );

        result.Items.Count.ShouldBeGreaterThanOrEqualTo(1);
        result.Items.ShouldContain(u => u.UserName == "neo");
    }
}
