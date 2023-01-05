using System;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.EntityFrameworkCore;

public class EfCoreDbContextTypeProvider : IEfCoreDbContextTypeProvider, ITransientDependency
{
    private readonly AbpDbContextOptions _options;
    private readonly ICurrentTenant _currentTenant;

    public EfCoreDbContextTypeProvider(IOptions<AbpDbContextOptions> options, ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
        _options = options.Value;
    }

    public virtual Type GetDbContextType(Type dbContextType)
    {
        return _options.GetReplacedTypeOrSelf(dbContextType, _currentTenant.GetMultiTenancySide());
    }
}
