using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.AuditLogging.MongoDB;

public class MongoAuditLogExcelFileRepository : MongoDbRepository<IAuditLoggingMongoDbContext, AuditLogExcelFile, Guid>, IAuditLogExcelFileRepository
{
    public MongoAuditLogExcelFileRepository(IMongoDbContextProvider<IAuditLoggingMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<AuditLogExcelFile>> GetListCreationTimeBeforeAsync(
        DateTime creationTimeBefore,
        int maxResultCount = 50,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync(cancellationToken);
        return await queryable
            .Where(x => x.CreationTime < creationTimeBefore)
            .OrderBy(x => x.CreationTime)
            .Take(maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}