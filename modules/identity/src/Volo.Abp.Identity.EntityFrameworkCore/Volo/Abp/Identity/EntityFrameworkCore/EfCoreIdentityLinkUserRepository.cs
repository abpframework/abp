using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    public class EfCoreIdentityLinkUserRepository : EfCoreRepository<IIdentityDbContext, IdentityLinkUser, Guid>, IIdentityLinkUserRepository
    {
        public EfCoreIdentityLinkUserRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<IdentityLinkUser> FindAsync(IdentityLinkUserInfo sourceLinkUserInfo, IdentityLinkUserInfo targetLinkUserInfo, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(x =>
                    x.SourceUserId == sourceLinkUserInfo.UserId && x.SourceTenantId == sourceLinkUserInfo.TenantId &&
                    x.TargetUserId == targetLinkUserInfo.UserId && x.TargetTenantId == targetLinkUserInfo.TenantId ||
                    x.TargetUserId == sourceLinkUserInfo.UserId && x.TargetTenantId == sourceLinkUserInfo.TenantId &&
                    x.SourceUserId == targetLinkUserInfo.UserId && x.SourceTenantId == targetLinkUserInfo.TenantId
                , cancellationToken: GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityLinkUser>> GetListAsync(IdentityLinkUserInfo linkUserInfo, bool includeIndirect = false, CancellationToken cancellationToken = default)
        {
            var linkUsers = await DbSet.Where(x =>
                    x.SourceUserId == linkUserInfo.UserId && x.SourceTenantId == linkUserInfo.TenantId ||
                    x.TargetUserId == linkUserInfo.UserId && x.TargetTenantId == linkUserInfo.TenantId)
                .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));

            if (!includeIndirect)
            {
                return linkUsers;
            }

            var excludeUsers = new List<IdentityLinkUserInfo>
            {
                linkUserInfo
            };

            var excludeExp = PredicateBuilder.New<IdentityLinkUser>(true);

            var indirectLinkUsers = linkUsers;
            while (indirectLinkUsers.Any())
            {
                foreach (var user in excludeUsers)
                {
                    excludeExp = excludeExp.And(PredicateBuilder.New<IdentityLinkUser>(x =>
                        (x.SourceUserId != user.UserId || x.SourceTenantId != user.TenantId) &&
                        (x.TargetUserId != user.UserId || x.TargetTenantId != user.TenantId)));
                }

                var includeExp = PredicateBuilder.New<IdentityLinkUser>(false);;
                foreach (var user in linkUsers.Select(x =>
                {
                    if (excludeUsers.Any(s => s.UserId == x.SourceUserId && s.TenantId == x.SourceTenantId))
                    {
                        return new IdentityLinkUserInfo(x.TargetUserId, x.TargetTenantId);
                    }

                    if (excludeUsers.Any(s => s.UserId == x.TargetUserId && s.TenantId == x.TargetTenantId))
                    {
                        return new IdentityLinkUserInfo(x.SourceUserId, x.SourceTenantId);
                    }

                    return null;
                }).Where(x => x != null))
                {
                    includeExp = includeExp.Or(PredicateBuilder.New<IdentityLinkUser>(x =>
                        x.SourceUserId == user.UserId && x.SourceTenantId == user.TenantId ||
                        x.TargetUserId == user.UserId && x.TargetTenantId == user.TenantId));

                    excludeUsers.Add(user);
                }

                indirectLinkUsers = await DbSet.Where(includeExp).Where(excludeExp).ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
                linkUsers.AddRange(indirectLinkUsers);
            }

            return linkUsers.Distinct().ToList();
        }


        public async Task DeleteAsync(IdentityLinkUserInfo linkUserInfo, CancellationToken cancellationToken = default)
        {
            var linkUsers = await DbSet.Where(x =>
                    x.SourceUserId == linkUserInfo.UserId && x.SourceTenantId == linkUserInfo.TenantId ||
                    x.TargetUserId == linkUserInfo.UserId && x.TargetTenantId == linkUserInfo.TenantId)
                .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));

            foreach (var user in linkUsers)
            {
                await DeleteAsync(user, cancellationToken: cancellationToken);
            }
        }
    }
}
