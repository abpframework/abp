using System;
using System.Collections.Generic;
using System.Linq;
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

        public virtual async Task<List<IdentityLinkUser>> GetListAsync(IdentityLinkUserInfo linkUserInfo, List<IdentityLinkUserInfo> excludes = null,
            CancellationToken cancellationToken = default)
        {
            var query = (await GetMongoQueryableAsync(cancellationToken)).Where(x =>
                x.SourceUserId == linkUserInfo.UserId && x.SourceTenantId == linkUserInfo.TenantId ||
                x.TargetUserId == linkUserInfo.UserId && x.TargetTenantId == linkUserInfo.TenantId);

            if (!excludes.IsNullOrEmpty())
            {
                foreach (var userInfo in excludes)
                {
                    query = query.Where(x =>
                        (x.SourceTenantId != userInfo.TenantId || x.SourceUserId != userInfo.UserId) &&
                        (x.TargetTenantId != userInfo.TenantId || x.TargetUserId != userInfo.UserId));
                }
            }

            return await query.ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task DeleteAsync(IdentityLinkUserInfo linkUserInfo, CancellationToken cancellationToken = default)
        {
            var linkUsers = await (await GetMongoQueryableAsync(cancellationToken)).Where(x =>
                    x.SourceUserId == linkUserInfo.UserId && x.SourceTenantId == linkUserInfo.TenantId ||
                    x.TargetUserId == linkUserInfo.UserId && x.TargetTenantId == linkUserInfo.TenantId)
                .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));

            await DeleteManyAsync(linkUsers, cancellationToken: cancellationToken);
        }
    }
}
