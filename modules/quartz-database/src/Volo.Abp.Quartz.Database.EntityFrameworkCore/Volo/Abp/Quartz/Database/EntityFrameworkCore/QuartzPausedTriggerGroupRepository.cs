using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{

    public class QuartzPausedTriggerGroupRepository : EfCoreRepository<IQuartzDatabaseDbContext, QuartzPausedTriggerGroup>,
        IQuartzPausedTriggerGroupRepository
    {
        public QuartzPausedTriggerGroupRepository(IDbContextProvider<IQuartzDatabaseDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
