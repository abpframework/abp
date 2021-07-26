using Microsoft.EntityFrameworkCore;
using System.Linq;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{

    public static class QuartzDatabaseEfCoreQueryableExtensions
    {
        public static IQueryable<QuartzTrigger> IncludeDetails(this IQueryable<QuartzTrigger> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.BlobTriggers)
                .Include(x => x.CronTriggers)
                .Include(x => x.SimpleTriggers)
                .Include(x => x.SimplePropertyTriggers);
        }

        public static IQueryable<QuartzJobDetail> IncludeDetails(this IQueryable<QuartzJobDetail> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Triggers).ThenInclude(x => x.BlobTriggers)
                .Include(x => x.Triggers).ThenInclude(x => x.CronTriggers)
                .Include(x => x.Triggers).ThenInclude(x => x.SimpleTriggers)
                .Include(x => x.Triggers).ThenInclude(x => x.SimplePropertyTriggers);
        }
    }
}