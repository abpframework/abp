using System;
using System.Collections.Generic;
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

        public async Task<List<AuditLog>> GetListAsync(string sorting = null, int maxResultCount = 50, int skipCount = 0,
            string httpMethod = null, string url = null, string userName = null, HttpStatusCode? httpStatusCode = null, bool includeDetails = true)
        {
            var query = GetListQuery(httpMethod, url, userName, httpStatusCode, includeDetails);

            return await query.OrderBy(sorting ?? "executionTime desc").As<IMongoQueryable<AuditLog>>()
                .PageBy<AuditLog, IMongoQueryable<AuditLog>>(skipCount, maxResultCount)
                .ToListAsync();
        }

        public async Task<long> GetCountAsync(string httpMethod = null, string url = null, string userName = null, HttpStatusCode? httpStatusCode = null, bool includeDetails = true)
        {
            var query = GetListQuery(httpMethod, url, userName, httpStatusCode, includeDetails);

            var count = await query.As<IMongoQueryable<AuditLog>>().LongCountAsync();

            return count;
        }

        private IQueryable<AuditLog> GetListQuery(
            string httpMethod = null,
            string url = null,
            string userName = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = true)
        {
            return GetMongoQueryable()
                .WhereIf(httpMethod != null, auditLog => auditLog.HttpMethod != null && auditLog.HttpMethod.ToLowerInvariant() == httpMethod.ToLowerInvariant())
                .WhereIf(url != null, auditLog => auditLog.Url != null && auditLog.Url.ToLowerInvariant().Contains(url.ToLowerInvariant()))
                .WhereIf(userName != null, auditLog => auditLog.UserName != null && auditLog.UserName == userName)
                .WhereIf(httpStatusCode != null && httpStatusCode > 0, auditLog => auditLog.HttpStatusCode == (int?)httpStatusCode);
        }
    }
}
