using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity;

// Multi-tenant separate-database isolation tests, runnable on any storage backend that lets
// each predefined tenant resolve to its own physical connection. EF (SQLite per-tenant
// keep-alive) and MongoDB (per-tenant database) implementations both work.
//
// NOTE on scope: open-source IdentityUserManager.FindSharedUserBy* assumes a single shared
// database; cross-DB shared-user resolution is the Pro UserSharingManager's responsibility.
// These tests therefore only assert the layer the open-source framework owns: tenant
// connection routing and IMultiTenant filter behavior.
//
// Concrete subclasses must register two predefined tenants (TenantAId / TenantBId from this
// class) each with its own connection string, and enable shared-user mode in the test module.
public abstract class IdentityUserManager_SharedUser_SeparateDatabase_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    public static readonly Guid TenantAId = IdentitySharedUserSeparateDbConstants.TenantAId;
    public static readonly Guid TenantBId = IdentitySharedUserSeparateDbConstants.TenantBId;

    protected IdentityUserManager IdentityUserManager { get; }
    protected IIdentityUserRepository IdentityUserRepository { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IDataFilter DataFilter { get; }

    protected IdentityUserManager_SharedUser_SeparateDatabase_Tests()
    {
        IdentityUserManager = GetRequiredService<IdentityUserManager>();
        IdentityUserRepository = GetRequiredService<IIdentityUserRepository>();
        CurrentTenant = GetRequiredService<ICurrentTenant>();
        UnitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        DataFilter = GetRequiredService<IDataFilter>();
    }

    [Fact]
    public virtual async Task Tenant_Connection_Should_Not_See_Host_Rows()
    {
        // Disables IMultiTenant before querying so this test fails if connection routing is
        // broken (a tenant context unexpectedly hitting the host db) rather than being masked
        // by the data filter.
        var probeEmail = $"infra-host-{Guid.NewGuid():N}@abp.io";
        Guid hostUserId;

        using (CurrentTenant.Change(null))
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            var hostUser = new IdentityUser(Guid.NewGuid(), $"infra-host-{Guid.NewGuid():N}", probeEmail, null);
            await IdentityUserRepository.InsertAsync(hostUser);
            hostUserId = hostUser.Id;
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(TenantAId))
        using (DataFilter.Disable<IMultiTenant>())
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            (await IdentityUserRepository.GetListAsync()).ShouldNotContain(u => u.Id == hostUserId);
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(TenantAId))
        {
            (await IdentityUserManager.FindByEmailAsync(probeEmail)).ShouldBeNull();
        }
    }

    [Fact]
    public virtual async Task Host_Connection_Should_Not_See_Tenant_Rows()
    {
        var probeEmail = $"infra-tenant-{Guid.NewGuid():N}@abp.io";
        Guid tenantUserId;

        using (CurrentTenant.Change(TenantAId))
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            var tenantUser = new IdentityUser(Guid.NewGuid(), $"infra-t-{Guid.NewGuid():N}", probeEmail, TenantAId);
            await IdentityUserRepository.InsertAsync(tenantUser);
            tenantUserId = tenantUser.Id;
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(null))
        using (DataFilter.Disable<IMultiTenant>())
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            (await IdentityUserRepository.GetListAsync()).ShouldNotContain(u => u.Id == tenantUserId);
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(null))
        {
            (await IdentityUserManager.FindByEmailAsync(probeEmail)).ShouldBeNull();
        }
    }

    [Fact]
    public virtual async Task TenantA_Connection_Should_Not_See_TenantB_Rows()
    {
        var probeEmail = $"infra-cross-{Guid.NewGuid():N}@abp.io";
        Guid tenantBUserId;

        using (CurrentTenant.Change(TenantBId))
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            var u = new IdentityUser(Guid.NewGuid(), $"b-{Guid.NewGuid():N}", probeEmail, TenantBId);
            await IdentityUserRepository.InsertAsync(u);
            tenantBUserId = u.Id;
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(TenantAId))
        using (DataFilter.Disable<IMultiTenant>())
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            (await IdentityUserRepository.GetListAsync()).ShouldNotContain(u => u.Id == tenantBUserId);
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(TenantAId))
        {
            (await IdentityUserManager.FindByEmailAsync(probeEmail)).ShouldBeNull();
        }
    }

    [Fact]
    public virtual async Task Different_Tenants_Should_Allow_Same_Email_With_Their_Own_Rows()
    {
        var email = $"same-email-{Guid.NewGuid():N}@abp.io";
        var nameA = $"a-{Guid.NewGuid():N}";
        var nameB = $"b-{Guid.NewGuid():N}";

        using (CurrentTenant.Change(TenantAId))
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            await IdentityUserRepository.InsertAsync(new IdentityUser(Guid.NewGuid(), nameA, email, TenantAId));
            await uow.CompleteAsync();
        }
        using (CurrentTenant.Change(TenantBId))
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            await IdentityUserRepository.InsertAsync(new IdentityUser(Guid.NewGuid(), nameB, email, TenantBId));
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(TenantAId))
        {
            var userA = await IdentityUserManager.FindByEmailAsync(email);
            userA.ShouldNotBeNull();
            userA.UserName.ShouldBe(nameA);
        }
        using (CurrentTenant.Change(TenantBId))
        {
            var userB = await IdentityUserManager.FindByEmailAsync(email);
            userB.ShouldNotBeNull();
            userB.UserName.ShouldBe(nameB);
        }
    }

    [Fact]
    public virtual async Task Different_Tenants_Should_Allow_Same_UserName_With_Their_Own_Rows()
    {
        var userName = $"same-name-{Guid.NewGuid():N}";
        var emailA = $"a-{Guid.NewGuid():N}@abp.io";
        var emailB = $"b-{Guid.NewGuid():N}@abp.io";

        using (CurrentTenant.Change(TenantAId))
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            await IdentityUserRepository.InsertAsync(new IdentityUser(Guid.NewGuid(), userName, emailA, TenantAId));
            await uow.CompleteAsync();
        }
        using (CurrentTenant.Change(TenantBId))
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            await IdentityUserRepository.InsertAsync(new IdentityUser(Guid.NewGuid(), userName, emailB, TenantBId));
            await uow.CompleteAsync();
        }

        using (CurrentTenant.Change(TenantAId))
        {
            var userInTenantA = await IdentityUserManager.FindByNameAsync(userName);
            userInTenantA.ShouldNotBeNull();
            userInTenantA.Email.ShouldBe(emailA);
        }
        using (CurrentTenant.Change(TenantBId))
        {
            var userInTenantB = await IdentityUserManager.FindByNameAsync(userName);
            userInTenantB.ShouldNotBeNull();
            userInTenantB.Email.ShouldBe(emailB);
        }
    }
}
