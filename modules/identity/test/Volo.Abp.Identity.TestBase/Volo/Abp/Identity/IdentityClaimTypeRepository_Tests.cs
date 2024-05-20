using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

public abstract class IdentityClaimTypeRepository_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IIdentityClaimTypeRepository ClaimTypeRepository { get; }
    protected IdentityClaimTypeManager IdentityClaimTypeManager { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IIdentityUserRepository UserRepository { get; }
    protected IdentityUserManager IdentityUserManager { get; }
    protected IIdentityRoleRepository RoleRepository { get; }
    protected IdentityRoleManager IdentityRoleManager { get; }
    protected IdentityTestData IdentityTestData { get; }

    public IdentityClaimTypeRepository_Tests()
    {
        ClaimTypeRepository = ServiceProvider.GetRequiredService<IIdentityClaimTypeRepository>();
        IdentityClaimTypeManager = ServiceProvider.GetRequiredService<IdentityClaimTypeManager>();
        GuidGenerator = ServiceProvider.GetRequiredService<IGuidGenerator>();
        UnitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        IdentityUserManager = ServiceProvider.GetRequiredService<IdentityUserManager>();
        UserRepository = ServiceProvider.GetRequiredService<IIdentityUserRepository>();
        RoleRepository = ServiceProvider.GetRequiredService<IIdentityRoleRepository>();
        IdentityRoleManager = ServiceProvider.GetRequiredService<IdentityRoleManager>();
        IdentityTestData = ServiceProvider.GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public async Task Should_Check_Name_If_It_Is_Uniquee()
    {
        var claim = (await ClaimTypeRepository.GetListAsync()).FirstOrDefault();

        var result1 = await ClaimTypeRepository.AnyAsync(claim.Name);

        result1.ShouldBe(true);

        var result2 = await ClaimTypeRepository.AnyAsync(Guid.NewGuid().ToString());

        result2.ShouldBe(false);
    }

    [Fact]
    public async Task GetCountAsync_With_Filter()
    {
        (await ClaimTypeRepository.GetCountAsync("Age")).ShouldBe(1);
    }

    [Fact]
    public async Task GetListAsyncByNames()
    {
        var result = await ClaimTypeRepository.GetListByNamesAsync(new List<string> { "Age", "Education" });

        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var ageClaim = await ClaimTypeRepository.FindAsync(IdentityTestData.AgeClaimId);
        ageClaim.ShouldNotBeNull();

        using (var uow = UnitOfWorkManager.Begin())
        {
            var john = await UserRepository.FindAsync(IdentityTestData.UserJohnId);
            john.ShouldNotBeNull();
            await IdentityUserManager.AddClaimAsync(john, new Claim(ageClaim.Name, "18"));
            var userClaims = await IdentityUserManager.GetClaimsAsync(john);
            userClaims.ShouldContain(c => c.Type == ageClaim.Name && c.Value == "18");

            var saleRole = await RoleRepository.FindAsync(IdentityTestData.RoleSaleId);
            saleRole.ShouldNotBeNull();
            await IdentityRoleManager.AddClaimAsync(saleRole, new Claim(ageClaim.Name, "18"));
            var roleClaims = await IdentityRoleManager.GetClaimsAsync(saleRole);
            roleClaims.ShouldContain(c => c.Type == ageClaim.Name && c.Value == "18");

            await uow.CompleteAsync();
        }

        await IdentityClaimTypeManager.DeleteAsync(ageClaim.Id);

        using (var uow = UnitOfWorkManager.Begin())
        {
            var john = await UserRepository.FindAsync(IdentityTestData.UserJohnId);
            john.ShouldNotBeNull();
            var userClaims = await IdentityUserManager.GetClaimsAsync(john);
            userClaims.ShouldNotContain(c => c.Type == ageClaim.Name && c.Value == "18");

            var saleRole = await RoleRepository.FindAsync(IdentityTestData.RoleSaleId);
            saleRole.ShouldNotBeNull();
            var roleClaims = await IdentityRoleManager.GetClaimsAsync(saleRole);
            roleClaims.ShouldNotContain(c => c.Type == ageClaim.Name && c.Value == "18");

            await uow.CompleteAsync();
        }
    }
}
