using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
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
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = true)
        {
            var query = GetListQuery(httpMethod, url, userName, httpStatusCode, includeDetails);

            var auditLogs = await query.OrderBy(sorting ?? "executionTime desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync();

            return auditLogs;
        }

        public async Task<long> GetCountAsync(
            string httpMethod = null,
            string url = null,
            string userName = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = true)
        {
            var query = GetListQuery(httpMethod, url, userName, httpStatusCode, includeDetails);

            var totalCount = await query.LongCountAsync();

            return totalCount;
        }

        private IQueryable<AuditLog> GetListQuery(
            string httpMethod = null,
            string url = null,
            string userName = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = true)
        {
            return DbSet.AsNoTracking()
                .IncludeDetails(includeDetails)
                .WhereIf(httpMethod != null, auditLog => auditLog.HttpMethod != null && auditLog.HttpMethod.ToLowerInvariant() == httpMethod.ToLowerInvariant())
                .WhereIf(url != null, auditLog => auditLog.Url != null && auditLog.Url.ToLowerInvariant().Contains(url.ToLowerInvariant()))
                .WhereIf(userName != null, auditLog => auditLog.UserName != null && auditLog.UserName == userName)
                .WhereIf(httpStatusCode != null && httpStatusCode > 0, auditLog => auditLog.HttpStatusCode == (int?)httpStatusCode);
        }

        public override IQueryable<AuditLog> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
