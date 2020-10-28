using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity
{
    public class IdentityLinkUserManager : DomainService
    {
        protected IIdentityLinkUserRepository IdentityLinkUserRepository { get; }

        protected IdentityUserManager UserManager { get; }

        protected new ICurrentTenant CurrentTenant { get; }

        public IdentityLinkUserManager(IIdentityLinkUserRepository identityLinkUserRepository, IdentityUserManager userManager, ICurrentTenant currentTenant)
        {
            IdentityLinkUserRepository = identityLinkUserRepository;
            UserManager = userManager;
            CurrentTenant = currentTenant;
        }

        public virtual async Task LinkAsync(IdentityLinkUserInfo sourceLinkUser, IdentityLinkUserInfo targetLinkUser)
        {
            if (sourceLinkUser.UserId == targetLinkUser.UserId && sourceLinkUser.TenantId == targetLinkUser.TenantId)
            {
                return;
            }

            if (await IsLinkedAsync(sourceLinkUser, targetLinkUser))
            {
                return;
            }

            using (CurrentTenant.Change(null))
            {
                var userLink = new IdentityLinkUser(
                    GuidGenerator.Create(),
                    sourceLinkUser,
                    targetLinkUser);
                await IdentityLinkUserRepository.InsertAsync(userLink, true);
            }
        }

        public virtual async Task<bool> IsLinkedAsync(IdentityLinkUserInfo sourceLinkUser, IdentityLinkUserInfo targetLinkUser)
        {
            using (CurrentTenant.Change(null))
            {
                return await IdentityLinkUserRepository.FindAsync(sourceLinkUser, targetLinkUser) != null;
            }
        }

        public virtual async Task UnlinkAsync(IdentityLinkUserInfo sourceLinkUser, IdentityLinkUserInfo targetLinkUser)
        {
            if (!await IsLinkedAsync(sourceLinkUser, targetLinkUser))
            {
                return;
            }

            using (CurrentTenant.Change(null))
            {
                var linkedUser = await IdentityLinkUserRepository.FindAsync(sourceLinkUser, targetLinkUser);
                if (linkedUser != null)
                {
                    await IdentityLinkUserRepository.DeleteAsync(linkedUser);
                }
            }
        }

        public virtual async Task<string> GenerateLinkTokenAsync(IdentityLinkUserInfo targetLinkUser)
        {
            using (CurrentTenant.Change(targetLinkUser.TenantId))
            {
                var user = await UserManager.GetByIdAsync(targetLinkUser.UserId);
                return await UserManager.GenerateUserTokenAsync(
                    user,
                    LinkUserTokenProvider.LinkUserTokenProviderName,
                    LinkUserTokenProvider.LinkUserTokenPurpose);
            }
        }

        public virtual async Task<bool> VerifyLinkTokenAsync(IdentityLinkUserInfo targetLinkUser, string token)
        {
            using (CurrentTenant.Change(targetLinkUser.TenantId))
            {
                var user = await UserManager.GetByIdAsync(targetLinkUser.UserId);
                return await UserManager.VerifyUserTokenAsync(
                    user,
                    LinkUserTokenProvider.LinkUserTokenProviderName,
                    LinkUserTokenProvider.LinkUserTokenPurpose,
                    token);
            }
        }
    }
}
