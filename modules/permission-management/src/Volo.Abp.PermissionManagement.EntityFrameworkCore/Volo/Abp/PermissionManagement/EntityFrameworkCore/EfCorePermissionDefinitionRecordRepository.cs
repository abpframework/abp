using System;
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
}