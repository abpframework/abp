using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
            return (await base.WithDetailsAsync()).Include(i => i.Items);
        }
    }
}
