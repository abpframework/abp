using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{
    public class QuartzSchedulerStateRepository : EfCoreRepository<IQuartzDatabaseDbContext, QuartzSchedulerState>,
        IQuartzSchedulerStateRepository
    {
        public QuartzSchedulerStateRepository(IDbContextProvider<IQuartzDatabaseDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
