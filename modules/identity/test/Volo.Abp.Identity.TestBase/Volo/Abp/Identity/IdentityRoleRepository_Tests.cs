using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

public abstract class IdentityRoleRepository_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IIdentityRoleRepository RoleRepository { get; }
    protected ILookupNormalizer LookupNormalizer { get; }
    protected IdentityUserManager UserManager { get; }
    protected IdentityTestData TestData { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected IdentityRoleRepository_Tests()
    {
        RoleRepository = ServiceProvider.GetRequiredService<IIdentityRoleRepository>();
        LookupNormalizer = ServiceProvider.GetRequiredService<ILookupNormalizer>();
        UserManager = ServiceProvider.GetRequiredService<IdentityUserManager>();
        TestData = ServiceProvider.GetRequiredService<IdentityTestData>();
        UnitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task FindByNormalizedNameAsync()
    {
        (await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName("admin"))).ShouldNotBeNull();
        (await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName("undefined-role"))).ShouldBeNull();
    }

    [Fact]
    public async Task GetListAsync()
    {
        var roles = await RoleRepository.GetListAsync();
        roles.ShouldContain(r => r.Name == "admin");
        roles.ShouldContain(r => r.Name == "moderator");
        roles.ShouldContain(r => r.Name == "supporter");
    }

    [Fact]
    public async Task GetDefaultOnesAsync()
    {
        var roles = await RoleRepository.GetDefaultOnesAsync();

        foreach (var role in roles)
        {
            role.IsDefault.ShouldBe(true);
        }
    }

    [Fact]
    public async Task GetCountAsync()
    {
        (await RoleRepository.GetCountAsync()).ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetCountAsync_With_Filter()
    {
        (await RoleRepository.GetCountAsync("admin")).ShouldBe(1);
    }

    [Fact]
    public async Task Should_Eager_Load_Role_Collections()
    {
        var role = await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName("moderator"));
        role.Claims.ShouldNotBeNull();
        role.Claims.Any().ShouldBeTrue();
    }

    [Fact]
    public async Task GetListWithUserCountAsync()
    {
        var roles = await RoleRepository.GetListWithUserCountAsync();

        roles.Count.ShouldBe(5);
        roles.ShouldContain(r => r.Role.Name == "admin" && r.UserCount == 2);
        roles.ShouldContain(r => r.Role.Name == "moderator" && r.UserCount == 1);
        roles.ShouldContain(r => r.Role.Name == "supporter" && r.UserCount == 2);
        roles.ShouldContain(r => r.Role.Name == "manager" && r.UserCount == 1);


        using (var uow = UnitOfWorkManager.Begin())
        {
            var userBob = await UserManager.FindByIdAsync(TestData.UserBobId.ToString());
            await UserManager.DeleteAsync(userBob!);
            await uow.CompleteAsync();
        }
        
        roles = await RoleRepository.GetListWithUserCountAsync();

        roles.Count.ShouldBe(5);
        roles.ShouldContain(r => r.Role.Name == "manager" && r.UserCount == 0);
        roles.ShouldContain(r => r.Role.Name == "sale" && r.UserCount == 0);
    }
}
