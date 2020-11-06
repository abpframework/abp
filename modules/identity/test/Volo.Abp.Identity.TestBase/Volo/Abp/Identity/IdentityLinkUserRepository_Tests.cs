using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Identity
{
    public abstract class IdentityLinkUserRepository_Tests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IIdentityUserRepository UserRepository { get; }
        protected IIdentityLinkUserRepository IdentityLinkUserRepository { get; }
        protected IdentityTestData TestData { get; }

        public IdentityLinkUserRepository_Tests()
        {
            UserRepository = GetRequiredService<IIdentityUserRepository>();
            IdentityLinkUserRepository = GetRequiredService<IIdentityLinkUserRepository>();
            TestData = GetRequiredService<IdentityTestData>();
        }

        [Fact]
        public async Task FindAsync()
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);
            var david = await UserRepository.GetAsync(TestData.UserDavidId);
            var neo = await UserRepository.GetAsync(TestData.UserNeoId);

            var johnAndDavidLinkUser = await IdentityLinkUserRepository.FindAsync(
                new IdentityLinkUserInfo(john.Id, john.TenantId),
                new IdentityLinkUserInfo(david.Id, david.TenantId));

            johnAndDavidLinkUser.ShouldNotBeNull();
            johnAndDavidLinkUser.SourceUserId.ShouldBe(john.Id);
            johnAndDavidLinkUser.SourceTenantId.ShouldBe(john.TenantId);
            johnAndDavidLinkUser.TargetUserId.ShouldBe(david.Id);
            johnAndDavidLinkUser.TargetTenantId.ShouldBe(david.TenantId);

            (await IdentityLinkUserRepository.FindAsync(
                new IdentityLinkUserInfo(john.Id, john.TenantId),
                new IdentityLinkUserInfo(neo.Id, neo.TenantId))).ShouldBeNull();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);
            var david = await UserRepository.GetAsync(TestData.UserDavidId);
            var neo = await UserRepository.GetAsync(TestData.UserNeoId);

            var davidLinkUsers = await IdentityLinkUserRepository.GetListAsync(new IdentityLinkUserInfo(david.Id, david.TenantId));
            davidLinkUsers.ShouldNotBeNull();

            davidLinkUsers.ShouldContain(x => x.SourceUserId == john.Id && x.SourceTenantId == john.TenantId);
            davidLinkUsers.ShouldContain(x => x.TargetUserId == neo.Id && x.TargetTenantId == neo.TenantId);
        }

        [Fact]
        public async Task GetList_Include_Indirect_Test()
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

            var linkUsers = await IdentityLinkUserRepository.GetListAsync(new IdentityLinkUserInfo(a, null), includeIndirect: true);
            linkUsers.Count.ShouldBe(5);

            linkUsers = await IdentityLinkUserRepository.GetListAsync(new IdentityLinkUserInfo(g, null), includeIndirect: true);
            linkUsers.Count.ShouldBe(2);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);
            var david = await UserRepository.GetAsync(TestData.UserDavidId);

            (await IdentityLinkUserRepository.FindAsync(
                new IdentityLinkUserInfo(john.Id, john.TenantId),
                new IdentityLinkUserInfo(david.Id, david.TenantId))).ShouldNotBeNull();

            await IdentityLinkUserRepository.DeleteAsync(new IdentityLinkUserInfo(david.Id, david.TenantId));

            (await IdentityLinkUserRepository.FindAsync(
                new IdentityLinkUserInfo(john.Id, john.TenantId),
                new IdentityLinkUserInfo(david.Id, david.TenantId))).ShouldBeNull();

            (await IdentityLinkUserRepository.FindAsync(
                new IdentityLinkUserInfo(david.Id, david.TenantId),
                new IdentityLinkUserInfo(john.Id, john.TenantId))).ShouldBeNull();
        }
    }
}
