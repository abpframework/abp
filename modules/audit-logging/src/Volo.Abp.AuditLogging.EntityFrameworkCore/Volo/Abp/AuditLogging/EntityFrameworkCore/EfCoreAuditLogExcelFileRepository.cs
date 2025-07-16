using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore;

public class EfCoreAuditLogExcelFileRepository : EfCoreRepository<IAuditLoggingDbContext, AuditLogExcelFile, Guid>, IAuditLogExcelFileRepository
{
    public EfCoreAuditLogExcelFileRepository(IDbContextProvider<IAuditLoggingDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<List<AuditLogExcelFile>> GetListCreationTimeBeforeAsync(
        DateTime creationTimeBefore,
        int maxResultCount = 50,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();
        return await queryable.Where(x => x.CreationTime < creationTimeBefore).Take(maxResultCount).ToListAsync(cancellationToken);
    }
}