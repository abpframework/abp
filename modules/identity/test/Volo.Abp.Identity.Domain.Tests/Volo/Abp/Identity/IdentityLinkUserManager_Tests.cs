using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity
{
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

        [Fact]
        public virtual async Task GenerateAndVerifyLinkTokenAsync()
        {
            var john = await UserRepository.GetAsync(TestData.UserJohnId);
            var token = await IdentityLinkUserManager.GenerateLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId));
            (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), token)).ShouldBeTrue();

            (await IdentityLinkUserManager.VerifyLinkTokenAsync(new IdentityLinkUserInfo(john.Id, john.TenantId), "123123")).ShouldBeFalse();
        }

    }
}
