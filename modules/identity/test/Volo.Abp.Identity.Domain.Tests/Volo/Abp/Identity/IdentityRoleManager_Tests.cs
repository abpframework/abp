using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityRoleManager_Tests : AbpIdentityDomainTestBase
{
    private readonly IdentityRoleManager _identityRoleManager;
    private readonly IIdentityRoleRepository _identityRoleRepository;
    private readonly ILookupNormalizer _lookupNormalizer;
    private readonly IdentityTestData _testData;

    public IdentityRoleManager_Tests()
    {
        _identityRoleManager = GetRequiredService<IdentityRoleManager>();
        _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
        _lookupNormalizer = GetRequiredService<ILookupNormalizer>();
        _testData = GetRequiredService<IdentityTestData>();
    }


    [Fact]
    public async Task GetByIdAsync()
    {
        var role = await _identityRoleManager.FindByIdAsync(_testData.RoleModeratorId.ToString());

        role.ShouldNotBeNull();
        role.Name.ShouldBe("moderator");
    }

    [Fact]
    public async Task SetRoleNameAsync()
    {
        var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
        role.ShouldNotBeNull();

        (await _identityRoleManager.SetRoleNameAsync(role, "teacher")).Succeeded.ShouldBeTrue();

        role.Name.ShouldBe("teacher");
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var role = await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"));
        role.ShouldNotBeNull();

        await _identityRoleManager.DeleteAsync(role);

        (await _identityRoleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("moderator"))).ShouldBeNull();
    }

}
