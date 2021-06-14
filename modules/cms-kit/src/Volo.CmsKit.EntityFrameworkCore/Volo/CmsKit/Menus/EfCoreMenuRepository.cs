using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Menus
{
    public class EfCoreMenuRepository : EfCoreRepository<ICmsKitDbContext, Menu, Guid>, IMenuRepository
    {
        public EfCoreMenuRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<Menu>> WithDetailsAsync()
        {
            return (await base.WithDetailsAsync()).IncludeDetails();
        }

        public virtual async Task<Menu> FindMainMenuAsync(bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await (includeDetails ? await WithDetailsAsync() : await GetQueryableAsync())
                .FirstOrDefaultAsync(x => x.IsMainMenu, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Menu>> GetCurrentAndNextMainMenusAsync(
            Guid nextMainMenuId, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await (includeDetails ? await WithDetailsAsync() : await GetQueryableAsync())
                .Where(x => x.IsMainMenu || x.Id == nextMainMenuId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
