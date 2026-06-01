using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

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

    public async Task<List<IdentityLinkUser>> GetListAsync(IdentityLinkUserInfo linkUserInfo, bool includeIndirect = false, int batchSize = 100 * 100, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(null))
        {
            var users = await IdentityLinkUserRepository.GetListAsync(linkUserInfo, cancellationToken: cancellationToken);
            if (!includeIndirect)
            {
                return users;
            }

            var allUsers = await IdentityLinkUserRepository.GetListAsync(batchSize ,cancellationToken: cancellationToken);
            return await GetAllRelatedLinksAsync(allUsers, linkUserInfo);
        }
    }

    protected virtual Task<List<IdentityLinkUser>> GetAllRelatedLinksAsync(List<IdentityLinkUser> allUsers, IdentityLinkUserInfo userInfo)
    {
        var visited = new HashSet<(Guid, Guid?)>();
        var result = new List<IdentityLinkUser>();
        var queue = new Queue<(Guid, Guid?)>();

        queue.Enqueue((userInfo.UserId, userInfo.TenantId));
        visited.Add((userInfo.UserId, userInfo.TenantId));

        while (queue.Count > 0)
        {
            var (currentUserId, currentTenantId) = queue.Dequeue();

            var relatedLinks = allUsers.Where(x =>
                (x.SourceUserId == currentUserId && x.SourceTenantId == currentTenantId) ||
                (x.TargetUserId == currentUserId && x.TargetTenantId == currentTenantId)
            ).ToList();

            foreach (var link in relatedLinks)
            {
                var node1 = (link.SourceUserId, link.SourceTenantId);
                var node2 = (link.TargetUserId, link.TargetTenantId);

                if (!result.Contains(link))
                {
                    result.Add(link);
                }

                if (!visited.Contains(node1))
                {
                    queue.Enqueue(node1);
                    visited.Add(node1);
                }

                if (!visited.Contains(node2))
                {
                    queue.Enqueue(node2);
                    visited.Add(node2);
                }
            }
        }

        return Task.FromResult(result);
    }

    public virtual async Task LinkAsync(IdentityLinkUserInfo sourceLinkUser, IdentityLinkUserInfo targetLinkUser, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(null))
        {
            if (sourceLinkUser.UserId == targetLinkUser.UserId && sourceLinkUser.TenantId == targetLinkUser.TenantId)
            {
                return;
            }

            if (await IsLinkedAsync(sourceLinkUser, targetLinkUser, cancellationToken: cancellationToken))
            {
                return;
            }

            var userLink = new IdentityLinkUser(
                GuidGenerator.Create(),
                sourceLinkUser,
                targetLinkUser);
            await IdentityLinkUserRepository.InsertAsync(userLink, true, cancellationToken);
        }
    }

    public virtual async Task<bool> IsLinkedAsync(IdentityLinkUserInfo sourceLinkUser, IdentityLinkUserInfo targetLinkUser, bool includeIndirect = false, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(null))
        {
            if (includeIndirect)
            {
                return (await GetListAsync(sourceLinkUser, true, cancellationToken: cancellationToken))
                    .Any(x => x.SourceTenantId == targetLinkUser.TenantId && x.SourceUserId == targetLinkUser.UserId ||
                              x.TargetTenantId == targetLinkUser.TenantId && x.TargetUserId == targetLinkUser.UserId);
            }
            return await IdentityLinkUserRepository.FindAsync(sourceLinkUser, targetLinkUser, cancellationToken) != null;
        }
    }

    public virtual async Task UnlinkAsync(IdentityLinkUserInfo sourceLinkUser, IdentityLinkUserInfo targetLinkUser, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(null))
        {
            if (!await IsLinkedAsync(sourceLinkUser, targetLinkUser, cancellationToken: cancellationToken))
            {
                return;
            }

            var linkedUser = await IdentityLinkUserRepository.FindAsync(sourceLinkUser, targetLinkUser, cancellationToken);
            if (linkedUser != null)
            {
                await IdentityLinkUserRepository.DeleteAsync(linkedUser, cancellationToken: cancellationToken);
            }
        }
    }

    public virtual async Task<string> GenerateLinkTokenAsync(IdentityLinkUserInfo targetLinkUser, string tokenPurpose, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(targetLinkUser.TenantId))
        {
            var user = await UserManager.GetByIdAsync(targetLinkUser.UserId);
            return await UserManager.GenerateUserTokenAsync(
                user,
                LinkUserTokenProviderConsts.LinkUserTokenProviderName,
                tokenPurpose);
        }
    }

    public virtual async Task<bool> VerifyLinkTokenAsync(IdentityLinkUserInfo targetLinkUser, string token, string tokenPurpose, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(targetLinkUser.TenantId))
        {
            var user = await UserManager.GetByIdAsync(targetLinkUser.UserId);
            return await UserManager.VerifyUserTokenAsync(
                user,
                LinkUserTokenProviderConsts.LinkUserTokenProviderName,
                tokenPurpose,
                token);
        }
    }
}
