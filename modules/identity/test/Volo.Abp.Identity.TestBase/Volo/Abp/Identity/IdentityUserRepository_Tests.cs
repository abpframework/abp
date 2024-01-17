using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Identity;

public abstract class IdentityUserRepository_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IIdentityUserRepository UserRepository { get; }
    protected IIdentityRoleRepository RoleRepository { get; }
    protected ILookupNormalizer LookupNormalizer { get; }
    protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
    protected OrganizationUnitManager OrganizationUnitManager { get; }
    protected IdentityTestData TestData { get; }

    protected IdentityUserRepository_Tests()
    {
        UserRepository = GetRequiredService<IIdentityUserRepository>();
        RoleRepository = GetRequiredService<IIdentityRoleRepository>();
        LookupNormalizer = GetRequiredService<ILookupNormalizer>();
        OrganizationUnitRepository = GetRequiredService<IOrganizationUnitRepository>();
        OrganizationUnitManager = GetRequiredService<OrganizationUnitManager>();;
        TestData = ServiceProvider.GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public async Task FindByNormalizedUserNameAsync()
    {
        (await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("john.nash"))).ShouldNotBeNull();
        (await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("undefined-user"))).ShouldBeNull();
    }

    [Fact]
    public async Task FindByNormalizedEmailAsync()
    {
        (await UserRepository.FindByNormalizedEmailAsync(LookupNormalizer.NormalizeEmail("john.nash@abp.io"))).ShouldNotBeNull();
        (await UserRepository.FindByNormalizedEmailAsync(LookupNormalizer.NormalizeEmail("david@abp.io"))).ShouldNotBeNull();
        (await UserRepository.FindByNormalizedEmailAsync(LookupNormalizer.NormalizeEmail("undefined-user@abp.io"))).ShouldBeNull();
    }

    [Fact]
    public async Task GetRoleNamesAsync()
    {
        var john = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("john.nash"));
        var roles = await UserRepository.GetRoleNamesAsync(john.Id);
        roles.Count.ShouldBe(3);
        roles.ShouldContain("moderator");
        roles.ShouldContain("supporter");
        roles.ShouldContain("manager");
    }

    [Fact]
    public async Task GetRoleNames_By_UserIds_Async()
    {
        var userRoleNames = await UserRepository.GetRoleNamesAsync(new [] {
            TestData.UserBobId,
            TestData.UserJohnId,
            TestData.UserNeoId,
            TestData.UserDavidId
        });

        userRoleNames.Count.ShouldBe(3);

        var userBob = userRoleNames.First(x => x.Id == TestData.UserBobId);
        userBob.RoleNames.Length.ShouldBe(1);
        userBob.RoleNames[0].ShouldBe("manager");

        var userJohn = userRoleNames.First(x => x.Id == TestData.UserJohnId);
        userJohn.RoleNames.Length.ShouldBe(3);
        userJohn.RoleNames.ShouldContain("moderator");
        userJohn.RoleNames.ShouldContain("supporter");
        userJohn.RoleNames.ShouldContain("manager");

        var userNeo = userRoleNames.First(x => x.Id == TestData.UserNeoId);
        userNeo.RoleNames.Length.ShouldBe(3);
        userNeo.RoleNames.ShouldContain("supporter");
        userJohn.RoleNames.ShouldContain("moderator");
        userJohn.RoleNames.ShouldContain("manager");
    }

    [Fact]
    public async Task FindByLoginAsync()
    {
        var user = await UserRepository.FindByLoginAsync("github", "john");
        user.ShouldNotBeNull();
        user.UserName.ShouldBe("john.nash");

        user = await UserRepository.FindByLoginAsync("twitter", "johnx");
        user.ShouldNotBeNull();
        user.UserName.ShouldBe("john.nash");

        (await UserRepository.FindByLoginAsync("github", "undefinedid")).ShouldBeNull();
    }

    [Fact]
    public async Task GetListByClaimAsync()
    {
        var users = await UserRepository.GetListByClaimAsync(new Claim("TestClaimType", "42"));
        users.Count.ShouldBe(2);
        users.ShouldContain(u => u.UserName == "administrator");
        users.ShouldContain(u => u.UserName == "john.nash");

        users = await UserRepository.GetListByClaimAsync(new Claim("TestClaimType", "43"));
        users.Count.ShouldBe(1);
        users.ShouldContain(u => u.UserName == "neo");

        users = await UserRepository.GetListByClaimAsync(new Claim("TestClaimType", "undefined"));
        users.Count.ShouldBe(0);
    }

    [Fact]
    public async Task GetListByNormalizedRoleNameAsync()
    {
        var users = await UserRepository.GetListByNormalizedRoleNameAsync(LookupNormalizer.NormalizeName("supporter"));
        users.Count.ShouldBe(2);
        users.ShouldContain(u => u.UserName == "john.nash");
        users.ShouldContain(u => u.UserName == "neo");
    }

    [Fact]
    public async Task GetUserIdListByRoleIdAsync()
    {
        var john = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("john.nash"));
        var neo = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("neo"));
        john.ShouldNotBeNull();
        neo.ShouldNotBeNull();

        var roleId = (await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName("supporter"))).Id;
        var users = await UserRepository.GetUserIdListByRoleIdAsync(roleId);
        users.Count.ShouldBe(2);
        users.ShouldContain(id => id == john.Id);
        users.ShouldContain(id => id == neo.Id);
    }

    [Fact]
    public async Task GetListAsync()
    {
        var users = await UserRepository.GetListAsync("UserName DESC", 5, 0, "n", isLockedOut: true);

        users.Count.ShouldBeGreaterThan(1);
        users.Count.ShouldBeLessThanOrEqualTo(5);

        //Filter check
        users.ShouldAllBe(u => u.UserName.Contains("n") || u.Email.Contains("n"));

        //Order check
        for (var i = 0; i < users.Count - 1; i++)
        {
            string.Compare(
                users[i].UserName,
                users[i + 1].UserName,
                StringComparison.OrdinalIgnoreCase
            ).ShouldBeGreaterThan(0);
        }
        
        users = await UserRepository.GetListAsync(null, 5, 0, null, roleId: TestData.RoleManagerId);
        users.ShouldContain(x => x.UserName == "john.nash");
        users.ShouldContain(x => x.UserName == "neo");

        users = await UserRepository.GetListAsync(null, 999, 0, "undefined-username");
        users.Count.ShouldBe(0);
    }

    [Fact]
    public async Task GetRolesAsync()
    {
        var john = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("john.nash"));
        var roles = await UserRepository.GetRolesAsync(john.Id);
        roles.Count.ShouldBe(3);
        roles.ShouldContain(r => r.Name == "moderator");
        roles.ShouldContain(r => r.Name == "supporter");
        roles.ShouldContain(r => r.Name == "manager");
    }

    [Fact]
    public async Task GetCountAsync()
    {
        (await UserRepository.GetCountAsync("n")).ShouldBeGreaterThan(1);
        (await UserRepository.GetCountAsync("undefined-username")).ShouldBe(0);
    }

    [Fact]
    public async Task GetUsersInOrganizationUnitAsync()
    {
        var users = await UserRepository.GetUsersInOrganizationUnitAsync((await GetOU("OU111")).Id);
        users.ShouldNotBeNull();
        users.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetUsersInOrganizationUnitWithChildrenAsync()
    {
        var users = await UserRepository.GetUsersInOrganizationUnitWithChildrenAsync((await GetOU("OU111")).Code);
        users.ShouldNotBeNull();
        users.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Eager_Load_User_Collections()
    {
        var john = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("john.nash"));

        john.Roles.ShouldNotBeNull();
        john.Roles.Any().ShouldBeTrue();

        john.Logins.ShouldNotBeNull();
        john.Logins.Any().ShouldBeTrue();

        john.Claims.ShouldNotBeNull();
        john.Claims.Any().ShouldBeTrue();

        john.Tokens.ShouldNotBeNull();
        john.Tokens.Any().ShouldBeTrue();

        john.OrganizationUnits.ShouldNotBeNull();
        john.OrganizationUnits.Any().ShouldBeTrue();
    }

    private async Task<OrganizationUnit> GetOU(string diplayName)
    {
        var organizationUnit = await OrganizationUnitRepository.GetAsync(diplayName);
        organizationUnit.ShouldNotBeNull();
        return organizationUnit;
    }

    [Fact]
    public async Task UpdateRolesAsync()
    {
        var john = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("john.nash"));
        var roles = await UserRepository.GetRolesAsync(john.Id);
        roles.Count.ShouldBe(3);
        roles.ShouldContain(r => r.Name == "moderator");
        roles.ShouldContain(r => r.Name == "supporter");
        roles.ShouldContain(r => r.Name == "manager");

        var supporter = roles.First(x => x.NormalizedName == LookupNormalizer.NormalizeName("supporter"));
        var manager = roles.First(x => x.NormalizedName == LookupNormalizer.NormalizeName("manager"));

        await UserRepository.UpdateRoleAsync(supporter.Id, null);

        roles = await UserRepository.GetRolesAsync(john.Id);
        roles.Count.ShouldBe(2);
        roles.ShouldContain(r => r.Name == "moderator");
        roles.ShouldContain(r => r.Name == "manager");

        var bob = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("bob"));
        roles = await UserRepository.GetRolesAsync(bob.Id);
        roles.Count.ShouldBe(1);
        roles.ShouldContain(r => r.Name == "manager");

        await UserRepository.UpdateRoleAsync(manager.Id, supporter.Id);

        roles = await UserRepository.GetRolesAsync(bob.Id);
        roles.Count.ShouldBe(1);
        roles.ShouldContain(r => r.Name == "supporter");
    }

    [Fact]
    public async Task UpdateOrganizationAsync()
    {
        var david = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("david"));
        var organizationUnits = await UserRepository.GetOrganizationUnitsAsync(david.Id);

        var ou111 = await OrganizationUnitRepository.GetAsync("OU111");
        var ou112 = await OrganizationUnitRepository.GetAsync("OU112");

        organizationUnits.Count.ShouldBe(1);
        organizationUnits.ShouldContain(r => r.Id == ou112.Id);

        await UserRepository.UpdateOrganizationAsync(ou112.Id, null);

        organizationUnits = await UserRepository.GetOrganizationUnitsAsync(david.Id);
        organizationUnits.Count.ShouldBe(0);

        var ou111Users = await UserRepository.GetUsersInOrganizationUnitAsync(ou111.Id);
        ou111Users.Count.ShouldBe(2);
        ou111Users.ShouldContain(x => x.UserName == "john.nash");
        ou111Users.ShouldContain(x => x.UserName == "neo");

        var ou112Users = await UserRepository.GetUsersInOrganizationUnitAsync(ou112.Id);
        ou112Users.Count.ShouldBe(0);

        await UserRepository.UpdateOrganizationAsync(ou111.Id, ou112.Id);

        ou112Users = await UserRepository.GetUsersInOrganizationUnitAsync(ou112.Id);
        ou112Users.Count.ShouldBe(2);
        ou112Users.ShouldContain(x => x.UserName == "john.nash");
        ou112Users.ShouldContain(x => x.UserName == "neo");
    }
}
