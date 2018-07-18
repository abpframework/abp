using System;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.AuditLogging
{
    public interface IAuditLogRepository : IBasicRepository<AuditLog, Guid>
    {
        Task<Paging.PagedResult<AuditLog>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50,
            int skipCount = 0,
            string filter = null,
            string httpMethod = null,
            string url = null,
            string userName = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = false);
    }
}
