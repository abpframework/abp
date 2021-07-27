using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{
    public class QuartzFiredTriggerRepository : EfCoreRepository<IQuartzDatabaseDbContext, QuartzFiredTrigger>,
        IQuartzFiredTriggerRepository
    {
        public QuartzFiredTriggerRepository(IDbContextProvider<IQuartzDatabaseDbContext> dbContextProvider)
           : base(dbContextProvider)
        {
        }
    }
}
