using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{
    public class QuartzTriggerRepository : EfCoreRepository<IQuartzDatabaseDbContext, QuartzTrigger>,
        IQuartzTriggerRepository
    {
        public QuartzTriggerRepository(IDbContextProvider<IQuartzDatabaseDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        [Obsolete("Use WithDetailsAsync method.")]
        public override IQueryable<QuartzTrigger> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public override async Task<IQueryable<QuartzTrigger>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
