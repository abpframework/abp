using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.FeatureManagement;

public interface IFeatureDefinitionRecordRepository : IBasicRepository<FeatureDefinitionRecord, Guid>
{
    Task<FeatureDefinitionRecord> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);
}
