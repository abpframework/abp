using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    public class EfCoreAuditLogRepository : EfCoreRepository<IAuditLoggingDbContext, AuditLog, Guid>, IAuditLogRepository
    {
        public EfCoreAuditLogRepository(IDbContextProvider<IAuditLoggingDbContext> dbContextProvider)
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
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = GetListQuery(httpMethod, url, userName, applicationName, correlationId, maxExecutionDuration, minExecutionDuration, httpStatusCode, includeDetails);

            var auditLogs = await query.OrderBy(sorting ?? "executionTime desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync();

            return auditLogs;
        }

        public async Task<long> GetCountAsync(
            string httpMethod = null,
            string url = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            HttpStatusCode? httpStatusCode = null,
            CancellationToken cancellationToken = default)
        {
            var query = GetListQuery(httpMethod, url, userName, applicationName, correlationId, maxExecutionDuration, minExecutionDuration, httpStatusCode);

            var totalCount = await query.LongCountAsync();

            return totalCount;
        }

        private IQueryable<AuditLog> GetListQuery(
            string httpMethod = null,
            string url = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = false)
        {
            return DbSet.AsNoTracking()
                .IncludeDetails(includeDetails)
                .WhereIf(httpMethod != null, auditLog => auditLog.HttpMethod != null && auditLog.HttpMethod.ToLowerInvariant() == httpMethod.ToLowerInvariant())
                .WhereIf(string.IsNullOrWhiteSpace(url), auditLog => auditLog.Url != null && auditLog.Url.ToLowerInvariant().Contains(url.ToLowerInvariant()))
                .WhereIf(string.IsNullOrWhiteSpace(userName), auditLog => auditLog.UserName != null && auditLog.UserName.ToLowerInvariant() == userName.ToLowerInvariant())
                .WhereIf(string.IsNullOrWhiteSpace(applicationName), auditLog => auditLog.ApplicationName != null && auditLog.ApplicationName.ToLowerInvariant() == applicationName.ToLowerInvariant())
                .WhereIf(string.IsNullOrWhiteSpace(correlationId), auditLog => auditLog.CorrelationId != null && auditLog.CorrelationId.ToLowerInvariant() == correlationId.ToLowerInvariant())
                .WhereIf(httpStatusCode != null && httpStatusCode > 0, auditLog => auditLog.HttpStatusCode == (int?)httpStatusCode)
                .WhereIf(maxExecutionDuration != null && maxExecutionDuration > 0, auditLog => auditLog.ExecutionDuration <= maxExecutionDuration)
                .WhereIf(minExecutionDuration != null && minExecutionDuration > 0, auditLog => auditLog.ExecutionDuration >= minExecutionDuration);
        }

        public override IQueryable<AuditLog> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
