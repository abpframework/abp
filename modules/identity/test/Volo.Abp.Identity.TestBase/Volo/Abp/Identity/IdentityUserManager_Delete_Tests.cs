using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

public abstract class IdentityUserManager_Delete_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IdentityUserManager IdentityUserManager { get; }
    protected IIdentityUserRepository IdentityUserRepository { get; }
    protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
    protected IdentityLinkUserManager IdentityLinkUserManager { get; }
    protected IIdentitySessionRepository IdentitySessionRepository { get; }
    protected IIdentityUserDelegationRepository IdentityUserDelegationRepository { get; }
    protected ILookupNormalizer LookupNormalizer { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IDataFilter DataFilter { get; }

    protected IdentityUserManager_Delete_Tests()
    {
        IdentityUserManager = GetRequiredService<IdentityUserManager>();
        IdentityUserRepository = GetRequiredService<IIdentityUserRepository>();
        OrganizationUnitRepository = GetRequiredService<IOrganizationUnitRepository>();
        IdentityLinkUserManager = GetRequiredService<IdentityLinkUserManager>();
        IdentitySessionRepository = GetRequiredService<IIdentitySessionRepository>();
        IdentityUserDelegationRepository = GetRequiredService<IIdentityUserDelegationRepository>();
        LookupNormalizer = GetRequiredService<ILookupNormalizer>();
        UnitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        DataFilter = GetRequiredService<IDataFilter>();
    }

    [Fact]
    public virtual async Task DeleteAsync_Should_Remove_All_Related_Data()
    {
        var userId = Guid.NewGuid();
        var linkedUserId = Guid.NewGuid();

        using (var uow = UnitOfWorkManager.Begin())
        {
            (await IdentityUserManager.CreateAsync(
                new IdentityUser(userId, $"delete-{userId:N}", $"delete-{userId:N}@abp.io"))).CheckErrors();
            (await IdentityUserManager.CreateAsync(
                new IdentityUser(linkedUserId, $"linked-{linkedUserId:N}", $"linked-{linkedUserId:N}@abp.io"))).CheckErrors();

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await IdentityUserManager.GetByIdAsync(userId);

            await IdentityUserManager.AddClaimAsync(user, new Claim("test", "test"));
            await IdentityUserManager.AddLoginAsync(user, new UserLoginInfo("test", "test", "test"));
            await IdentityUserManager.AddToRoleAsync(user, "moderator");
            user.SetToken("test", "test", "test");
            user.AddPasswordHistory("test");
            user.AddPasskey([1, 2, 3], new IdentityPasskeyData());
            await IdentityUserManager.AddToOrganizationUnitAsync(
                user,
                await OrganizationUnitRepository.GetAsync(LookupNormalizer.NormalizeName("OU11")));
            await IdentityLinkUserManager.LinkAsync(
                new IdentityLinkUserInfo(userId),
                new IdentityLinkUserInfo(linkedUserId));

            await IdentitySessionRepository.InsertAsync(new IdentitySession(
                Guid.NewGuid(), $"session-{userId:N}", "Web", "Chrome", userId, null, "MyApp", "127.0.0.1", DateTime.UtcNow));
            await IdentityUserDelegationRepository.InsertAsync(new IdentityUserDelegation(
                Guid.NewGuid(), userId, linkedUserId, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)));
            await IdentityUserDelegationRepository.InsertAsync(new IdentityUserDelegation(
                Guid.NewGuid(), linkedUserId, userId, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)));

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await IdentityUserManager.GetByIdAsync(userId);

            user.Claims.Count.ShouldBeGreaterThan(0);
            user.Logins.Count.ShouldBeGreaterThan(0);
            user.Roles.Count.ShouldBeGreaterThan(0);
            user.Tokens.Count.ShouldBeGreaterThan(0);
            user.OrganizationUnits.Count.ShouldBeGreaterThan(0);
            user.PasswordHistories.Count.ShouldBeGreaterThan(0);
            user.Passkeys.Count.ShouldBeGreaterThan(0);

            (await IdentityUserManager.DeleteAsync(user)).CheckErrors();

            await uow.CompleteAsync();
        }

        //The user is soft deleted, disable the filter to see what is left behind.
        using (var uow = UnitOfWorkManager.Begin())
        using (DataFilter.Disable<ISoftDelete>())
        {
            var deletedUser = await IdentityUserRepository.FindAsync(userId);
            deletedUser.ShouldNotBeNull();

            deletedUser.Claims.Count.ShouldBe(0);
            deletedUser.Logins.Count.ShouldBe(0);
            deletedUser.Roles.Count.ShouldBe(0);
            deletedUser.Tokens.Count.ShouldBe(0);
            deletedUser.OrganizationUnits.Count.ShouldBe(0);
            deletedUser.PasswordHistories.Count.ShouldBe(0);
            deletedUser.Passkeys.Count.ShouldBe(0);

            (await IdentityLinkUserManager.IsLinkedAsync(
                new IdentityLinkUserInfo(userId),
                new IdentityLinkUserInfo(linkedUserId))).ShouldBeFalse();

            (await IdentitySessionRepository.GetCountAsync(userId: userId)).ShouldBe(0);
            (await IdentityUserDelegationRepository.GetListAsync(sourceUserId: userId, targetUserId: null)).ShouldBeEmpty();
            (await IdentityUserDelegationRepository.GetListAsync(sourceUserId: null, targetUserId: userId)).ShouldBeEmpty();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public virtual async Task DeleteAsync_Should_Remove_Related_Data_Of_A_User_Loaded_Without_Details()
    {
        var userId = Guid.NewGuid();
        var userName = $"no-details-{userId:N}";

        using (var uow = UnitOfWorkManager.Begin())
        {
            (await IdentityUserManager.CreateAsync(
                new IdentityUser(userId, userName, $"{userName}@abp.io"))).CheckErrors();

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await IdentityUserManager.GetByIdAsync(userId);
            await IdentityUserManager.AddToRoleAsync(user, "moderator");
            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await IdentityUserRepository.FindByNormalizedUserNameAsync(
                LookupNormalizer.NormalizeName(userName),
                includeDetails: false);

            (await IdentityUserManager.DeleteAsync(user)).CheckErrors();

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        using (DataFilter.Disable<ISoftDelete>())
        {
            var deletedUser = await IdentityUserRepository.FindAsync(userId);
            deletedUser.ShouldNotBeNull();
            deletedUser.Roles.Count.ShouldBe(0);

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public virtual async Task Deleting_A_User_Through_The_Repository_Should_Remove_Sessions_Delegations_And_Links()
    {
        var userId = Guid.NewGuid();
        var linkedUserId = Guid.NewGuid();

        using (var uow = UnitOfWorkManager.Begin())
        {
            (await IdentityUserManager.CreateAsync(
                new IdentityUser(userId, $"repo-{userId:N}", $"repo-{userId:N}@abp.io"))).CheckErrors();
            (await IdentityUserManager.CreateAsync(
                new IdentityUser(linkedUserId, $"repo-linked-{linkedUserId:N}", $"repo-linked-{linkedUserId:N}@abp.io"))).CheckErrors();

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            await IdentityLinkUserManager.LinkAsync(
                new IdentityLinkUserInfo(userId),
                new IdentityLinkUserInfo(linkedUserId));

            await IdentitySessionRepository.InsertAsync(new IdentitySession(
                Guid.NewGuid(), $"repo-session-{userId:N}", "Web", "Chrome", userId, null, "MyApp", "127.0.0.1", DateTime.UtcNow));
            await IdentityUserDelegationRepository.InsertAsync(new IdentityUserDelegation(
                Guid.NewGuid(), userId, linkedUserId, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)));

            await uow.CompleteAsync();
        }

        //Custom code may delete the user without using IdentityUserManager.
        using (var uow = UnitOfWorkManager.Begin())
        {
            await IdentityUserRepository.DeleteAsync(await IdentityUserRepository.GetAsync(userId));
            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            (await IdentitySessionRepository.GetCountAsync(userId: userId)).ShouldBe(0);
            (await IdentityUserDelegationRepository.GetListAsync(sourceUserId: userId, targetUserId: null)).ShouldBeEmpty();
            (await IdentityLinkUserManager.IsLinkedAsync(
                new IdentityLinkUserInfo(userId),
                new IdentityLinkUserInfo(linkedUserId))).ShouldBeFalse();

            await uow.CompleteAsync();
        }
    }
}
