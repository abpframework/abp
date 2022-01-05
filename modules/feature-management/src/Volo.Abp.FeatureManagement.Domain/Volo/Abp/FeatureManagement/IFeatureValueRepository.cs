using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.FeatureManagement;

public interface IFeatureValueRepository : IBasicRepository<FeatureValue, Guid>
{
    Task<FeatureValue> FindAsync(
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default);

    Task<List<FeatureValue>> FindAllAsync(
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default);

    Task<List<FeatureValue>> GetListAsync(
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default);
}
