using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.PermissionManagement.EntityFrameworkCore;

public class EfCorePermissionDefinitionRecordRepository :
    EfCoreRepository<IPermissionManagementDbContext, PermissionDefinitionRecord, Guid>,
    IPermissionDefinitionRecordRepository
{
    public EfCorePermissionDefinitionRecordRepository(
        IDbContextProvider<IPermissionManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public virtual async Task<PermissionDefinitionRecord> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }
}