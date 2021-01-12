using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoIdentityLinkUserRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityLinkUser, Guid>, IIdentityLinkUserRepository
    {
        public MongoIdentityLinkUserRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<IdentityLinkUser> FindAsync(IdentityLinkUserInfo sourceLinkUserInfo, IdentityLinkUserInfo targetLinkUserInfo, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken))
                .OrderBy(x => x.Id).FirstOrDefaultAsync(x =>
                    x.SourceUserId == sourceLinkUserInfo.UserId && x.SourceTenantId == sourceLinkUserInfo.TenantId &&
                    x.TargetUserId == targetLinkUserInfo.UserId && x.TargetTenantId == targetLinkUserInfo.TenantId ||
                    x.TargetUserId == sourceLinkUserInfo.UserId && x.TargetTenantId == sourceLinkUserInfo.TenantId &&
                    x.SourceUserId == targetLinkUserInfo.UserId && x.SourceTenantId == targetLinkUserInfo.TenantId
                , cancellationToken: GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityLinkUser>> GetListAsync(IdentityLinkUserInfo linkUserInfo, bool includeIndirect = false, CancellationToken cancellationToken = default)
        {
            var linkUsers = await (await GetMongoQueryableAsync(cancellationToken)).Where(x =>
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

            Expression<Func<IdentityLinkUser, bool>> excludeExp = null;

            var indirectLinkUsers = linkUsers;
            while (indirectLinkUsers.Any())
            {
                foreach (var user in excludeUsers)
                {
                    if (excludeExp == null)
                    {
                        excludeExp = PredicateBuilder.New<IdentityLinkUser>(x =>
                            (x.SourceUserId != user.UserId || x.SourceTenantId != user.TenantId) &&
                            (x.TargetUserId != user.UserId || x.TargetTenantId != user.TenantId));
                    }
                    else
                    {
                        excludeExp = excludeExp.And(PredicateBuilder.New<IdentityLinkUser>(x =>
                            (x.SourceUserId != user.UserId || x.SourceTenantId != user.TenantId) &&
                            (x.TargetUserId != user.UserId || x.TargetTenantId != user.TenantId)));
                    }
                }

                Expression<Func<IdentityLinkUser, bool>> includeExp = null;
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
                    if (includeExp == null)
                    {
                        includeExp = PredicateBuilder.New<IdentityLinkUser>(x =>
                            x.SourceUserId == user.UserId && x.SourceTenantId == user.TenantId ||
                            x.TargetUserId == user.UserId && x.TargetTenantId == user.TenantId);
                    }
                    else
                    {
                        includeExp = includeExp.Or(PredicateBuilder.New<IdentityLinkUser>(x =>
                            x.SourceUserId == user.UserId && x.SourceTenantId == user.TenantId ||
                            x.TargetUserId == user.UserId && x.TargetTenantId == user.TenantId));
                    }

                    excludeUsers.Add(user);
                }

                indirectLinkUsers = await GetMongoQueryable().Where(includeExp).Where(excludeExp).ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
                linkUsers.AddRange(indirectLinkUsers);
            }

            return linkUsers.Distinct().ToList();
        }

        public async Task DeleteAsync(IdentityLinkUserInfo linkUserInfo, CancellationToken cancellationToken = default)
        {
            var linkUsers = await GetMongoQueryable().Where(x =>
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
