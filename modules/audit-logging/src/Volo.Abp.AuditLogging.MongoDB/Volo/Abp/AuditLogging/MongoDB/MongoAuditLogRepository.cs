using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.AuditLogging.MongoDB
{
    public class MongoAuditLogRepository : MongoDbRepository<IAuditLoggingMongoDbContext, AuditLog, Guid>, IAuditLogRepository
    {
        public MongoAuditLogRepository(IMongoDbContextProvider<IAuditLoggingMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<Paging.PagedResult<AuditLog>> GetListAsync(string sorting = null, int maxResultCount = 50, int skipCount = 0, string filter = null,
            string httpMethod = null, string url = null, HttpStatusCode? httpStatusCode = null, bool includeDetails = false)
        {
            var query = GetMongoQueryable()
                .WhereIf(httpMethod != null, q => q.HttpMethod == httpMethod)
                .WhereIf(url != null, q => q.Url == url)
                .WhereIf(httpStatusCode != null && httpStatusCode > 0, q => q.HttpStatusCode == (int?)httpStatusCode);

            var totalCount = await query.As<IMongoQueryable<AuditLog>>().LongCountAsync();

            var questions = await query.OrderBy(sorting).As<IMongoQueryable<AuditLog>>()
                .PageBy<AuditLog, IMongoQueryable<AuditLog>>(skipCount, maxResultCount)
                .ToListAsync();

            return new Paging.PagedResult<AuditLog>(totalCount, questions);
        }
    }
}
