using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.FeatureManagement.EntityFrameworkCore;

public class EfCoreFeatureValueRepository : EfCoreRepository<IFeatureManagementDbContext, FeatureValue, Guid>, IFeatureValueRepository
{
    public EfCoreFeatureValueRepository(IDbContextProvider<IFeatureManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<FeatureValue> FindAsync(
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey, GetCancellationToken(cancellationToken));
    }

    public async Task<List<FeatureValue>> FindAllAsync(
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(
                s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<FeatureValue>> GetListAsync(
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(
                s => s.ProviderName == providerName && s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }
}
