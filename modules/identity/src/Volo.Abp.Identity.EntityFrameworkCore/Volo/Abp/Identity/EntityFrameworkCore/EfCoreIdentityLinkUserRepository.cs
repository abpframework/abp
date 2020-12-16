using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
            return await DbSet
                .OrderBy(x => x.Id).FirstOrDefaultAsync(x =>
                    x.SourceUserId == sourceLinkUserInfo.UserId && x.SourceTenantId == sourceLinkUserInfo.TenantId &&
                    x.TargetUserId == targetLinkUserInfo.UserId && x.TargetTenantId == targetLinkUserInfo.TenantId ||
                    x.TargetUserId == sourceLinkUserInfo.UserId && x.TargetTenantId == sourceLinkUserInfo.TenantId &&
                    x.SourceUserId == targetLinkUserInfo.UserId && x.SourceTenantId == targetLinkUserInfo.TenantId
                , cancellationToken: GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityLinkUser>> GetListAsync(IdentityLinkUserInfo linkUserInfo, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(x =>
                    x.SourceUserId == linkUserInfo.UserId && x.SourceTenantId == linkUserInfo.TenantId ||
                    x.TargetUserId == linkUserInfo.UserId && x.TargetTenantId == linkUserInfo.TenantId)
                .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
        }
    }
}
