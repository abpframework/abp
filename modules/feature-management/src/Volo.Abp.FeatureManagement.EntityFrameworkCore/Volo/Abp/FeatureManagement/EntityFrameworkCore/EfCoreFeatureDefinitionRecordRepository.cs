using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.FeatureManagement.EntityFrameworkCore;

public class EfCoreFeatureDefinitionRecordRepository :
    EfCoreRepository<IFeatureManagementDbContext, FeatureDefinitionRecord, Guid>,
    IFeatureDefinitionRecordRepository
{
    public EfCoreFeatureDefinitionRecordRepository(
        IDbContextProvider<IFeatureManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
