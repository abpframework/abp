using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{
    public class QuartzJobDetailRepository : EfCoreRepository<IQuartzDatabaseDbContext, QuartzJobDetail>,
         IQuartzJobDetailRepository
    {
        public QuartzJobDetailRepository(IDbContextProvider<IQuartzDatabaseDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        [Obsolete("Use WithDetailsAsync method.")]
        public override IQueryable<QuartzJobDetail> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public override async Task<IQueryable<QuartzJobDetail>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
