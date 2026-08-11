using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
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
    protected ICurrentTenant CurrentTenant { get; }
    protected IDistributedCache<AbpDynamicClaimCacheItem> DynamicClaimCache { get; }

    protected IdentityUserManager_Delete_Tests()
    {
        CurrentTenant = GetRequiredService<ICurrentTenant>();
        DynamicClaimCache = GetRequiredService<IDistributedCache<AbpDynamicClaimCacheItem>>();
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

            (await IdentityUserManager.AddClaimAsync(user, new Claim("test", "test"))).CheckErrors();
            (await IdentityUserManager.AddLoginAsync(user, new UserLoginInfo("test", "test", "test"))).CheckErrors();
            (await IdentityUserManager.AddToRoleAsync(user, "moderator")).CheckErrors();
            user.SetToken("test", "test", "test");
            user.AddPasswordHistory("test");
            user.AddPasskey([1, 2, 3], new IdentityPasskeyData());
            await IdentityUserManager.AddToOrganizationUnitAsync(
                user,
                await OrganizationUnitRepository.GetAsync(LookupNormalizer.NormalizeName("OU11")));
            await IdentityUserRepository.UpdateAsync(user);

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

        //Every collection of the user has to be loaded before it is cleared.
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

    [Fact]
    public virtual async Task Should_Delete_A_User_That_Does_Not_Pass_The_User_Validators()
    {
        var userId = Guid.NewGuid();

        //Insert with the repository, so the user name is not validated.
        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = new IdentityUser(userId, $"invalid user name {userId:N}", $"invalid-{userId:N}@abp.io");
            user.AddPasswordHistory("test");
            await IdentityUserRepository.InsertAsync(user);

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            (await IdentityUserManager.DeleteAsync(await IdentityUserRepository.GetAsync(userId))).CheckErrors();

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        using (DataFilter.Disable<ISoftDelete>())
        {
            var deletedUser = await IdentityUserRepository.FindAsync(userId);
            deletedUser.IsDeleted.ShouldBeTrue();
            deletedUser.PasswordHistories.Count.ShouldBe(0);

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public virtual async Task Should_Remove_The_Related_Data_Of_A_Tenant_User_When_The_Unit_Of_Work_Completes_In_The_Host()
    {
        var tenantId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        using (var uow = UnitOfWorkManager.Begin())
        using (CurrentTenant.Change(tenantId))
        {
            await IdentityUserRepository.InsertAsync(
                new IdentityUser(userId, $"tenant-{userId:N}", $"tenant-{userId:N}@abp.io", tenantId));
            await IdentitySessionRepository.InsertAsync(new IdentitySession(
                Guid.NewGuid(), $"tenant-session-{userId:N}", "Web", "Chrome", userId, tenantId, "MyApp", "127.0.0.1", DateTime.UtcNow));
            await IdentityUserDelegationRepository.InsertAsync(new IdentityUserDelegation(
                Guid.NewGuid(), userId, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1), tenantId));

            await uow.CompleteAsync();
        }

        //The event is published while the unit of work completes, the current tenant is the host then.
        using (var uow = UnitOfWorkManager.Begin())
        {
            using (CurrentTenant.Change(tenantId))
            {
                await IdentityUserRepository.DeleteAsync(await IdentityUserRepository.GetAsync(userId));
            }

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        using (CurrentTenant.Change(tenantId))
        {
            (await IdentitySessionRepository.GetCountAsync(userId: userId)).ShouldBe(0);
            (await IdentityUserDelegationRepository.GetListAsync(sourceUserId: userId, targetUserId: null)).ShouldBeEmpty();

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public virtual async Task Should_Remove_The_Dynamic_Claims_Cache_Of_A_Deleted_User()
    {
        var userId = Guid.NewGuid();
        var cacheKey = AbpDynamicClaimCacheItem.CalculateCacheKey(userId, null);

        using (var uow = UnitOfWorkManager.Begin())
        {
            (await IdentityUserManager.CreateAsync(
                new IdentityUser(userId, $"claims-cache-{userId:N}", $"claims-cache-{userId:N}@abp.io"))).CheckErrors();

            await uow.CompleteAsync();
        }

        await DynamicClaimCache.SetAsync(cacheKey, new AbpDynamicClaimCacheItem());
        (await DynamicClaimCache.GetAsync(cacheKey)).ShouldNotBeNull();

        using (var uow = UnitOfWorkManager.Begin())
        {
            (await IdentityUserManager.DeleteAsync(await IdentityUserRepository.GetAsync(userId))).CheckErrors();

            await uow.CompleteAsync();
        }

        (await DynamicClaimCache.GetAsync(cacheKey)).ShouldBeNull();
    }

    [Fact]
    public virtual async Task Deleting_A_Stale_User_Should_Throw_A_Concurrency_Exception()
    {
        var userId = Guid.NewGuid();
        IdentityUser staleUser;

        using (var uow = UnitOfWorkManager.Begin())
        {
            (await IdentityUserManager.CreateAsync(
                new IdentityUser(userId, $"stale-{userId:N}", $"stale-{userId:N}@abp.io"))).CheckErrors();

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            staleUser = await IdentityUserRepository.GetAsync(userId);
            await uow.CompleteAsync();
        }

        //Change the user, so the instance loaded above has an old concurrency stamp.
        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await IdentityUserRepository.GetAsync(userId);
            user.Name = "Changed";
            await IdentityUserRepository.UpdateAsync(user);
            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            await Should.ThrowAsync<AbpIdentityResultException>(
                async () => await IdentityUserManager.DeleteAsync(staleUser));
        }
    }

    [Fact]
    public virtual async Task Deleting_A_User_Through_The_Repository_Should_Not_Clear_Its_Own_Collections()
    {
        var userId = Guid.NewGuid();

        using (var uow = UnitOfWorkManager.Begin())
        {
            (await IdentityUserManager.CreateAsync(
                new IdentityUser(userId, $"repo-collections-{userId:N}", $"repo-collections-{userId:N}@abp.io"))).CheckErrors();

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await IdentityUserManager.GetByIdAsync(userId);
            (await IdentityUserManager.AddToRoleAsync(user, "moderator")).CheckErrors();
            user.AddPasswordHistory("test");
            await IdentityUserRepository.UpdateAsync(user);

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            await IdentityUserRepository.DeleteAsync(await IdentityUserRepository.GetAsync(userId));

            await uow.CompleteAsync();
        }

        //UserDeletedEventHandler only covers the aggregates that have no navigation from the user,
        //the collections of the user itself are cleared by IdentityUserManager.DeleteAsync.
        using (var uow = UnitOfWorkManager.Begin())
        using (DataFilter.Disable<ISoftDelete>())
        {
            var deletedUser = await IdentityUserRepository.FindAsync(userId);
            deletedUser.Roles.Count.ShouldBe(1);
            deletedUser.PasswordHistories.Count.ShouldBe(1);

            await uow.CompleteAsync();
        }
    }

    [Fact]
    public virtual async Task Deleting_A_Stale_User_Should_Not_Delete_Its_Link_Users()
    {
        var userId = Guid.NewGuid();
        var linkedUserId = Guid.NewGuid();
        IdentityUser staleUser;

        using (var uow = UnitOfWorkManager.Begin())
        {
            (await IdentityUserManager.CreateAsync(
                new IdentityUser(userId, $"stale-link-{userId:N}", $"stale-link-{userId:N}@abp.io"))).CheckErrors();
            (await IdentityUserManager.CreateAsync(
                new IdentityUser(linkedUserId, $"stale-linked-{linkedUserId:N}", $"stale-linked-{linkedUserId:N}@abp.io"))).CheckErrors();

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            await IdentityLinkUserManager.LinkAsync(
                new IdentityLinkUserInfo(userId),
                new IdentityLinkUserInfo(linkedUserId));

            staleUser = await IdentityUserRepository.GetAsync(userId);

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            var user = await IdentityUserRepository.GetAsync(userId);
            user.Name = "Changed";
            await IdentityUserRepository.UpdateAsync(user);

            await uow.CompleteAsync();
        }

        using (var uow = UnitOfWorkManager.Begin())
        {
            await Should.ThrowAsync<AbpIdentityResultException>(
                async () => await IdentityUserManager.DeleteAsync(staleUser));
        }

        //The user is still there, so its link users must be there as well.
        using (var uow = UnitOfWorkManager.Begin())
        {
            (await IdentityLinkUserManager.IsLinkedAsync(
                new IdentityLinkUserInfo(userId),
                new IdentityLinkUserInfo(linkedUserId))).ShouldBeTrue();

            await uow.CompleteAsync();
        }
    }
}
