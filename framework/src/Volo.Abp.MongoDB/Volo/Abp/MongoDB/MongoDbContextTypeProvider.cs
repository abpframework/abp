using System;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.MongoDB;

public class MongoDbContextTypeProvider : IMongoDbContextTypeProvider, ITransientDependency
{
    private readonly AbpMongoDbContextOptions _options;
    private readonly ICurrentTenant _currentTenant;

    public MongoDbContextTypeProvider(IOptions<AbpMongoDbContextOptions> options, ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
        _options = options.Value;
    }

    public virtual Type GetDbContextType(Type dbContextType)
    {
        return _options.GetReplacedTypeOrSelf(dbContextType, _currentTenant.GetMultiTenancySide());
    }
}
