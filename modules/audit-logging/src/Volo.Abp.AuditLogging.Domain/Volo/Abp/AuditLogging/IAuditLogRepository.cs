using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.AuditLogging
{
    public interface IAuditLogRepository : IBasicRepository<AuditLog, Guid>
    {
        Task<List<AuditLog>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50,
            int skipCount = 0,
            string httpMethod = null,
            string url = null,
            string userName = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = true);

        Task<long> GetCountAsync(
            string httpMethod = null,
            string url = null,
            string userName = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = true);
    }
}
