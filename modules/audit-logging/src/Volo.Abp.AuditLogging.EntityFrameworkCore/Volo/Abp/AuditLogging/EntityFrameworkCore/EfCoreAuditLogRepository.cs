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
            DateTime? startTime = null,
            DateTime? endTime = null,
            string httpMethod = null,
            string url = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            bool? hasException = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = GetListQuery(
                startTime,
                endTime,
                httpMethod,
                url,
                userName,
                applicationName,
                correlationId,
                maxExecutionDuration,
                minExecutionDuration,
                hasException,
                httpStatusCode,
                includeDetails
            );

            var auditLogs = await query.OrderBy(sorting ?? "executionTime desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));

            return auditLogs;
        }

        public async Task<long> GetCountAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string httpMethod = null,
            string url = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            bool? hasException = null,
            HttpStatusCode? httpStatusCode = null,
            CancellationToken cancellationToken = default)
        {
            var query = GetListQuery(
                startTime,
                endTime,
                httpMethod,
                url,
                userName,
                applicationName,
                correlationId,
                maxExecutionDuration,
                minExecutionDuration,
                hasException,
                httpStatusCode
            );

            var totalCount = await query.LongCountAsync(GetCancellationToken(cancellationToken));

            return totalCount;
        }

        private IQueryable<AuditLog> GetListQuery(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string httpMethod = null,
            string url = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            bool? hasException = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = false)
        {
            return DbSet.AsNoTracking()
                .IncludeDetails(includeDetails)
                .WhereIf(startTime.HasValue, auditLog => auditLog.ExecutionTime >= startTime)
                .WhereIf(endTime.HasValue, auditLog => auditLog.ExecutionTime <= endTime)
                .WhereIf(hasException.HasValue && hasException.Value, auditLog => auditLog.Exceptions != null && auditLog.Exceptions != "")
                .WhereIf(hasException.HasValue && !hasException.Value, auditLog => auditLog.Exceptions == null || auditLog.Exceptions == "")
                .WhereIf(httpMethod != null, auditLog => auditLog.HttpMethod == httpMethod)
                .WhereIf(url != null, auditLog => auditLog.Url != null && auditLog.Url.Contains(url))
                .WhereIf(userName != null, auditLog => auditLog.UserName == userName)
                .WhereIf(applicationName != null, auditLog => auditLog.ApplicationName == applicationName)
                .WhereIf(correlationId != null, auditLog => auditLog.CorrelationId == correlationId)
                .WhereIf(httpStatusCode != null && httpStatusCode > 0, auditLog => auditLog.HttpStatusCode == (int?)httpStatusCode)
                .WhereIf(maxExecutionDuration != null && maxExecutionDuration.Value > 0, auditLog => auditLog.ExecutionDuration <= maxExecutionDuration)
                .WhereIf(minExecutionDuration != null && minExecutionDuration.Value > 0, auditLog => auditLog.ExecutionDuration >= minExecutionDuration);
        }

        public async Task<Dictionary<DateTime, double>> GetAverageExecutionDurationPerDayAsync(DateTime startDate, DateTime endDate)
        {
            var result = await DbSet.AsNoTracking()
                .Where(a => a.ExecutionTime < endDate.AddDays(1) && a.ExecutionTime > startDate)
                .OrderBy(t => t.ExecutionTime)
                .GroupBy(t => new { t.ExecutionTime.Date })
                .Select(g => new { Day = g.Min(t => t.ExecutionTime), avgExecutionTime = g.Average(t => t.ExecutionDuration) })
                .ToListAsync();

            return result.ToDictionary(element => element.Day.ClearTime(), element => element.avgExecutionTime);
        }

        public override IQueryable<AuditLog> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
