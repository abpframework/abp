using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{
    public class QuartzCalendarRepository : EfCoreRepository<IQuartzDatabaseDbContext, QuartzCalendar>,
        IQuartzCalendarRepository
    {
        public QuartzCalendarRepository(IDbContextProvider<IQuartzDatabaseDbContext> dbContextProvider)
           : base(dbContextProvider)
        {
        }
    }
}
