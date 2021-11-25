using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityLinkUserManager_Tests : AbpIdentityDomainTestBase
{
    protected IIdentityUserRepository UserRepository { get; }
    protected IIdentityLinkUserRepository IdentityLinkUserRepository { get; }
    protected IdentityLinkUserManager IdentityLinkUserManager { get; }
    protected IdentityTestData TestData { get; }

    public IdentityLinkUserManager_Tests()
    {
        UserRepository = GetRequiredService<IIdentityUserRepository>();
        IdentityLinkUserRepository = GetRequiredService<IIdentityLinkUserRepository>();
        IdentityLinkUserManager = GetRequiredService<IdentityLinkUserManager>();
        TestData = GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public async Task GetListAsync_Test()
    {
        var a = Guid.NewGuid();
        var b = Guid.NewGuid();
        var c = Guid.NewGuid();
        var d = Guid.NewGuid();
        var e = Guid.NewGuid();
        var f = Guid.NewGuid();
        var g = Guid.NewGuid();
        var h = Guid.NewGuid();
        var i = Guid.NewGuid();

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(a, null),
            new IdentityLinkUserInfo(b, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(c, null),
            new IdentityLinkUserInfo(a, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(d, null),
            new IdentityLinkUserInfo(c, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(e, null),
            new IdentityLinkUserInfo(c, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(f, null),
            new IdentityLinkUserInfo(e, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(g, null),
            new IdentityLinkUserInfo(h, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(i, null),
            new IdentityLinkUserInfo(h, null)), true);

        var linkUsers = await IdentityLinkUserManager.GetListAsync(new IdentityLinkUserInfo(a, null));
        linkUsers.Count.ShouldBe(2);

        linkUsers = await IdentityLinkUserManager.GetListAsync(new IdentityLinkUserInfo(f, null));
        linkUsers.Count.ShouldBe(1);

        linkUsers = await IdentityLinkUserManager.GetListAsync(new IdentityLinkUserInfo(g, null));
        linkUsers.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetListAsync_Indirect_Test()
    {
        var a = Guid.NewGuid();
        var b = Guid.NewGuid();
        var c = Guid.NewGuid();
        var d = Guid.NewGuid();
        var e = Guid.NewGuid();
        var f = Guid.NewGuid();
        var g = Guid.NewGuid();
        var h = Guid.NewGuid();
        var i = Guid.NewGuid();

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(a, null),
            new IdentityLinkUserInfo(b, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(c, null),
            new IdentityLinkUserInfo(a, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(d, null),
            new IdentityLinkUserInfo(c, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(e, null),
            new IdentityLinkUserInfo(c, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(f, null),
            new IdentityLinkUserInfo(e, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(g, null),
            new IdentityLinkUserInfo(h, null)), true);

        await IdentityLinkUserRepository.InsertAsync(new IdentityLinkUser(
            Guid.NewGuid(),
            new IdentityLinkUserInfo(i, null),
            new IdentityLinkUserInfo(h, null)), true);

        var linkUsers = await IdentityLinkUserManager.GetListAsync(new IdentityLinkUserInfo(a, null), includeIndirect: true);
        linkUsers.Count.ShouldBe(5);

        linkUsers = await IdentityLinkUserManager.GetListAsync(new IdentityLinkUserInfo(f, null), includeIndirect: true);
        linkUsers.Count.ShouldBe(5);

        linkUsers = await IdentityLinkUserManager.GetListAsync(new IdentityLinkUserInfo(g, null), includeIndirect: true);
        linkUsers.Count.ShouldBe(2);
    }

    [Fact]
    public virtual async Task LinkAsync()
    {
        var john = await UserRepository.GetAsync(TestData.UserJohnId);
        var neo = await UserRepository.GetAsync(TestData.UserNeoId);

        (await IdentityLinkUserRepository.FindAsync(new IdentityLinkUserInfo(john.Id, john.TenantId),
            new IdentityLinkUserInfo(neo.Id, neo.TenantId))).ShouldBeNull();

        await IdentityLinkUserManager.LinkAsync(new IdentityLinkUserInfo(john.Id, john.TenantId),
            new IdentityLinkUserInfo(neo.Id, neo.TenantId));

        var linkUser = await IdentityLinkUserRepository.FindAsync(new IdentityLinkUserInfo(john.Id, john.TenantId),
            new IdentityLinkUserInfo(neo.Id, neo.TenantId));

        linkUser.ShouldNotBeNull();
        linkUser.SourceUserId.ShouldBe(john.Id);
        linkUser.SourceTenantId.ShouldBe(john.TenantId);

        linkUser.TargetUserId.ShouldBe(neo.Id);
        linkUser.TargetTenantId.ShouldBe(neo.TenantId);
    }

    [Fact]
    public virtual async Task UnlinkAsync()
    {
        var john = await UserRepository.GetAsync(TestData.UserJohnId);
        var david = await UserRepository.GetAsync(TestData.UserDavidId);

        (await IdentityLinkUserRepository.FindAsync(new IdentityLinkUserInfo(john.Id, john.TenantId),
            new IdentityLinkUserInfo(david.Id, david.TenantId))).ShouldNotBeNull();

        await IdentityLinkUserManager.UnlinkAsync(new IdentityLinkUserInfo(john.Id, john.TenantId),
            new IdentityLinkUserInfo(david.Id, david.TenantId));

        (await IdentityLinkUserRepository.FindAsync(new IdentityLinkUserInfo(john.Id, john.TenantId),
            new IdentityLinkUserInfo(david.Id, david.TenantId))).ShouldBeNull();
    }

    [Fact]
    public virtual async Task IsLinkedAsync()
    {
        var john = await UserRepository.GetAsync(TestData.UserJohnId);
        var david = await UserRepository.GetAsync(TestData.UserDavidId);
        var neo = await UserRepository.GetAsync(TestData.UserNeoId);

        (await IdentityLinkUserManager.IsLinkedAsync(new IdentityLinkUserInfo(john.Id, john.TenantId),
            new IdentityLinkUserInfo(david.Id, david.TenantId))).ShouldBeTrue();

        (await IdentityLinkUserManager.IsLinkedAsync(new IdentityLinkUserInfo(john.Id, john.TenantId),
            new IdentityLinkUserInfo(neo.Id, neo.TenantId))).ShouldBeFalse();
    }
}
