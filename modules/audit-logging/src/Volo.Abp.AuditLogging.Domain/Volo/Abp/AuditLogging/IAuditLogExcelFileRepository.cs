using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.AuditLogging;

public interface IAuditLogExcelFileRepository : IRepository<AuditLogExcelFile, Guid>
{
    Task<List<AuditLogExcelFile>> GetListCreationTimeBeforeAsync(
        DateTime creationTimeBefore,
        int maxResultCount = 50,
        CancellationToken cancellationToken = default);
}