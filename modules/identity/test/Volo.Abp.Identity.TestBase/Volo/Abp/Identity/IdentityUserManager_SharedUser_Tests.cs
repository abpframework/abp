using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

// Abstract test suite for IdentityUserManager.FindSharedUserBy*Async under
// TenantUserSharingStrategy.Shared. Concrete subclasses in Domain.Tests (EF) and
// MongoDB.Tests pick the storage backend by passing the corresponding TStartupModule.
public abstract class IdentityUserManager_SharedUser_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IdentityUserManager IdentityUserManager { get; }
    protected IIdentityUserRepository IdentityUserRepository { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected IdentityUserManager_SharedUser_Tests()
    {
        IdentityUserManager = GetRequiredService<IdentityUserManager>();
        IdentityUserRepository = GetRequiredService<IIdentityUserRepository>();
        CurrentTenant = GetRequiredService<ICurrentTenant>();
        UnitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
            options.UserSharingStrategy = TenantUserSharingStrategy.Shared;
        });
    }

    [Fact]
    public virtual async Task FindSharedUserByEmailAsync_Should_Return_Host_User()
    {
        var tenantId = Guid.NewGuid();
        var email = $"shared-email-{Guid.NewGuid():N}@abp.io";

        using (var uow = UnitOfWorkManager.Begin())
        {
            await CreateUserAsync(null, $"shared-host-email-{Guid.NewGuid():N}", email);
            await CreateUserAsync(tenantId, $"shared-tenant-email-{Guid.NewGuid():N}", email);
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(tenantId))
        {
            var user = await IdentityUserManager.FindSharedUserByEmailAsync(email);
            user.ShouldNotBeNull();
            user.TenantId.ShouldBeNull();
        }
    }

    [Fact]
    public virtual async Task FindSharedUserByEmailAsync_Should_Find_Tenant_User_When_No_Host_User()
    {
        var tenantId = Guid.NewGuid();
        var email = $"shared-tenant-only-{Guid.NewGuid():N}@abp.io";

        using (var uow = UnitOfWorkManager.Begin())
        {
            await CreateUserAsync(tenantId, $"shared-tenant-only-{Guid.NewGuid():N}", email);
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(null))
        {
            var user = await IdentityUserManager.FindSharedUserByEmailAsync(email);
            user.ShouldNotBeNull();
            user.TenantId.ShouldBe(tenantId);
        }
    }

    [Fact]
    public virtual async Task FindSharedUserByEmailAsync_Should_Return_Null_For_Unknown_Email()
    {
        using (CurrentTenant.Change(null))
        {
            var user = await IdentityUserManager.FindSharedUserByEmailAsync($"missing-{Guid.NewGuid():N}@abp.io");
            user.ShouldBeNull();
        }
    }

    [Fact]
    public virtual async Task FindSharedUserByNameAsync_Should_Return_Host_User()
    {
        var tenantId = Guid.NewGuid();
        var userName = $"shared-name-{Guid.NewGuid():N}";

        using (var uow = UnitOfWorkManager.Begin())
        {
            await CreateUserAsync(null, userName, $"host-{Guid.NewGuid():N}@abp.io");
            await CreateUserAsync(tenantId, userName, $"tenant-{Guid.NewGuid():N}@abp.io");
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(tenantId))
        {
            var user = await IdentityUserManager.FindSharedUserByNameAsync(userName);
            user.ShouldNotBeNull();
            user.TenantId.ShouldBeNull();
        }
    }

    [Fact]
    public virtual async Task FindSharedUserByLoginAsync_Should_Return_Host_User()
    {
        var tenantId = Guid.NewGuid();
        var loginProvider = "github";
        var providerKey = $"shared-login-{Guid.NewGuid():N}";

        using (var uow = UnitOfWorkManager.Begin())
        {
            await CreateUserAsync(null, $"host-login-{Guid.NewGuid():N}", $"host-login-{Guid.NewGuid():N}@abp.io",
                u => u.AddLogin(new UserLoginInfo(loginProvider, providerKey, "Shared Login")));
            await CreateUserAsync(tenantId, $"tenant-login-{Guid.NewGuid():N}", $"tenant-login-{Guid.NewGuid():N}@abp.io",
                u => u.AddLogin(new UserLoginInfo(loginProvider, providerKey, "Shared Login")));
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(tenantId))
        {
            var user = await IdentityUserManager.FindSharedUserByLoginAsync(loginProvider, providerKey);
            user.ShouldNotBeNull();
            user.TenantId.ShouldBeNull();
        }
    }

    [Fact]
    public virtual async Task FindSharedUserByPasskeyIdAsync_Should_Return_Host_User()
    {
        var tenantId = Guid.NewGuid();
        var credentialId = Guid.NewGuid().ToByteArray();

        using (var uow = UnitOfWorkManager.Begin())
        {
            await CreateUserAsync(null, $"shared-host-passkey-{Guid.NewGuid():N}", $"shared-host-passkey-{Guid.NewGuid():N}@abp.io",
                u => u.AddPasskey(credentialId, new IdentityPasskeyData()));
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(tenantId))
        {
            var user = await IdentityUserManager.FindSharedUserByPasskeyIdAsync(credentialId);
            user.ShouldNotBeNull();
            user.TenantId.ShouldBeNull();
        }
    }

    [Fact]
    public virtual async Task FindSharedUserByIdAsync_Should_Find_Tenant_User_From_Host_Context()
    {
        // Core 2FA shared-mode bug condition: a tenant-only user must be reachable by id from
        // a host context (CurrentTenant=null). The IMultiTenant filter would otherwise hide it.
        var tenantId = Guid.NewGuid();
        IdentityUser tenantUser;

        using (var uow = UnitOfWorkManager.Begin())
        {
            tenantUser = await CreateUserAsync(tenantId, $"shared-id-tenant-{Guid.NewGuid():N}", $"shared-id-tenant-{Guid.NewGuid():N}@abp.io");
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(null))
        {
            var user = await IdentityUserManager.FindSharedUserByIdAsync(tenantUser.Id.ToString());
            user.ShouldNotBeNull();
            user.Id.ShouldBe(tenantUser.Id);
            user.TenantId.ShouldBe(tenantId);
        }
    }

    [Fact]
    public virtual async Task FindSharedUserByIdAsync_Should_Find_Host_User_From_Tenant_Context()
    {
        var tenantId = Guid.NewGuid();
        IdentityUser hostUser;

        using (var uow = UnitOfWorkManager.Begin())
        {
            hostUser = await CreateUserAsync(null, $"shared-id-host-{Guid.NewGuid():N}", $"shared-id-host-{Guid.NewGuid():N}@abp.io");
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(tenantId))
        {
            var user = await IdentityUserManager.FindSharedUserByIdAsync(hostUser.Id.ToString());
            user.ShouldNotBeNull();
            user.TenantId.ShouldBeNull();
        }
    }

    [Fact]
    public virtual async Task FindSharedUserByIdAsync_Should_Return_Null_For_Unknown_Id()
    {
        using (CurrentTenant.Change(null))
        {
            var user = await IdentityUserManager.FindSharedUserByIdAsync(Guid.NewGuid().ToString());
            user.ShouldBeNull();
        }
    }

    [Fact]
    public virtual async Task Login_Then_TwoFactor_MidFlow_Should_Resolve_Same_Tenant_User()
    {
        // End-to-end shape of the 2FA shared-mode regression: a host-context lookup-by-name
        // followed by lookup-by-id must return the same tenant row both times.
        var tenantId = Guid.NewGuid();
        var userName = $"shared-2fa-{Guid.NewGuid():N}";

        using (var uow = UnitOfWorkManager.Begin())
        {
            await CreateUserAsync(tenantId, userName, $"{userName}@abp.io");
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(null))
        {
            var loginUser = await IdentityUserManager.FindSharedUserByNameAsync(userName);
            loginUser.ShouldNotBeNull();
            loginUser.TenantId.ShouldBe(tenantId);

            var twoFactorUser = await IdentityUserManager.FindSharedUserByIdAsync(loginUser.Id.ToString());
            twoFactorUser.ShouldNotBeNull();
            twoFactorUser.Id.ShouldBe(loginUser.Id);
            twoFactorUser.TenantId.ShouldBe(tenantId);
        }
    }

    protected async Task<IdentityUser> CreateUserAsync(
        Guid? tenantId,
        string userName,
        string email,
        Action<IdentityUser> configureUser = null)
    {
        var user = new IdentityUser(Guid.NewGuid(), userName, email, tenantId);
        configureUser?.Invoke(user);

        using (CurrentTenant.Change(tenantId))
        {
            await IdentityUserRepository.InsertAsync(user);
        }

        return user;
    }
}
