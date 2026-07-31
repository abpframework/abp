using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.EntityFrameworkCore;

public class IdentityUserRepository_Tests : IdentityUserRepository_Tests<AbpIdentityEntityFrameworkCoreTestModule>
{
    [Fact]
    public async Task UpdateLastSignInTimeAsync_Should_Be_Deferred_In_A_Transactional_UnitOfWork()
    {
        var userManager = ServiceProvider.GetRequiredService<IdentityUserManager>();
        var unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        var user = new IdentityUser(Guid.NewGuid(), "bob.lee", "bob.lee@abp.io");

        using (var uow = unitOfWorkManager.Begin(new AbpUnitOfWorkOptions
               {
                   IsTransactional = true
               }, requiresNew: true))
        {
            (await userManager.CreateAsync(user)).CheckErrors();
            await userManager.UpdateLastSignInTimeAsync(user.Id);

            await uow.CompleteAsync();
        }

        var createdUser = await UserRepository.FindAsync(user.Id);
        createdUser.ShouldNotBeNull();
        createdUser.LastSignInTime.ShouldNotBeNull();
    }
}
