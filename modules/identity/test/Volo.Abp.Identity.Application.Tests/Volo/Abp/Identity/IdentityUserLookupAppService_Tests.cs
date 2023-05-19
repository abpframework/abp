using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityUserLookupAppService_Tests : AbpIdentityApplicationTestBase
{
    private readonly IIdentityUserLookupAppService _identityUserLookupAppService;
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly ILookupNormalizer _lookupNormalizer;

    public IdentityUserLookupAppService_Tests()
    {
        _identityUserLookupAppService = GetRequiredService<IIdentityUserLookupAppService>();
        _identityUserRepository = GetRequiredService<IIdentityUserRepository>();
        _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
    }

    [Fact]
    public async Task FindByIdAsync()
    {
        var user = await _identityUserRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserLookupAppService.FindByIdAsync(user.Id)).UserName.ShouldBe(user.UserName);
    }

    [Fact]
    public async Task FindById_NotExist_Should_Return_Null()
    {
        var user = await _identityUserLookupAppService.FindByIdAsync(Guid.NewGuid());
        user.ShouldBeNull();
    }

    [Fact]
    public async Task FindByUserNameAsync()
    {
        var user = await _identityUserRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("john.nash"));
        user.ShouldNotBeNull();

        (await _identityUserLookupAppService.FindByUserNameAsync(user.UserName)).UserName.ShouldBe(user.UserName);
    }

    [Fact]
    public async Task FindByUserName_NotExist_Should_Return_Null()
    {
        var user = await _identityUserLookupAppService.FindByUserNameAsync(Guid.NewGuid().ToString());
        user.ShouldBeNull();
    }

    [Fact]
    public async Task Search_Without_Filter_And_Sorting()
    {
        var result = await _identityUserLookupAppService.SearchAsync(new UserLookupSearchInputDto());
        result.Items.Count.ShouldBeGreaterThanOrEqualTo(3);
        result.Items.ShouldContain(u => u.UserName == "john.nash");
    }

    [Fact]
    public async Task Search_With_Filter()
    {
        var result = await _identityUserLookupAppService.SearchAsync(
            new UserLookupSearchInputDto
            {
                Filter = "a"
            }
        );

        result.Items.Count.ShouldBeGreaterThanOrEqualTo(2);
        result.Items.ShouldContain(u => u.UserName == "john.nash");
        result.Items.ShouldContain(u => u.UserName == "david");

        result = await _identityUserLookupAppService.SearchAsync(
            new UserLookupSearchInputDto
            {
                Filter = "neo"
            }
        );

        result.Items.Count.ShouldBeGreaterThanOrEqualTo(1);
        result.Items.ShouldContain(u => u.UserName == "neo");
    }
}
