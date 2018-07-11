using System;
using System.Linq;
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

        public override IQueryable<AuditLog> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
