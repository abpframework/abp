using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading;
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

        public async Task<List<AuditLog>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50, 
            int skipCount = 0,
            string httpMethod = null,
            string url = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            int? maxDuration = null,
            int? minDuration = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = GetListQuery(httpMethod, url, userName, applicationName, correlationId, maxDuration, minDuration, httpStatusCode, includeDetails);

            return await query.OrderBy(sorting ?? "executionTime desc").As<IMongoQueryable<AuditLog>>()
                .PageBy<AuditLog, IMongoQueryable<AuditLog>>(skipCount, maxResultCount)
                .ToListAsync();
        }

        public async Task<long> GetCountAsync(
            string httpMethod = null,
            string url = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            int? maxDuration = null,
            int? minDuration = null,
            HttpStatusCode? httpStatusCode = null,
            CancellationToken cancellationToken = default)
        {
            var query = GetListQuery(httpMethod, url, userName, applicationName, correlationId, maxDuration, minDuration, httpStatusCode);

            var count = await query.As<IMongoQueryable<AuditLog>>().LongCountAsync();

            return count;
        }

        private IQueryable<AuditLog> GetListQuery(
            string httpMethod = null,
            string url = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            int? maxDuration = null,
            int? minDuration = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = false)
        {
            return GetMongoQueryable()
                .WhereIf(httpMethod != null, auditLog => auditLog.HttpMethod != null && auditLog.HttpMethod.ToLowerInvariant() == httpMethod.ToLowerInvariant())
                .WhereIf(url != null, auditLog => auditLog.Url != null && auditLog.Url.ToLowerInvariant().Contains(url.ToLowerInvariant()))
                .WhereIf(userName != null, auditLog => auditLog.UserName != null && auditLog.UserName.ToLowerInvariant() == userName.ToLowerInvariant())
                .WhereIf(applicationName != null, auditLog => auditLog.ApplicationName != null && auditLog.ApplicationName.ToLowerInvariant() == applicationName.ToLowerInvariant())
                .WhereIf(correlationId != null, auditLog => auditLog.CorrelationId != null && auditLog.CorrelationId.ToLowerInvariant() == correlationId.ToLowerInvariant())
                .WhereIf(httpStatusCode != null && httpStatusCode > 0, auditLog => auditLog.HttpStatusCode == (int?)httpStatusCode)
                .WhereIf(maxDuration != null && maxDuration > 0, auditLog => auditLog.ExecutionDuration <= maxDuration)
                .WhereIf(minDuration != null && minDuration > 0, auditLog => auditLog.ExecutionDuration >= minDuration); ;
        }
    }
}
