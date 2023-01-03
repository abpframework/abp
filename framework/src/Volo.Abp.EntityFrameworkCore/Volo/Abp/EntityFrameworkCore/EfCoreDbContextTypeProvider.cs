using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore;

public class EfCoreDbContextTypeProvider : IEfCoreDbContextTypeProvider, ITransientDependency
{
    private readonly AbpDbContextOptions _options;

    public EfCoreDbContextTypeProvider(IOptions<AbpDbContextOptions> options)
    {
        _options = options.Value;
    }

    public Type GetDbContextType(Type dbContextType)
    {
        return _options.GetReplacedTypeOrSelf(dbContextType);
    }

    public virtual Task<Type> GetDbContextTypeAsync(Type dbContextType)
    {
        return Task.FromResult(_options.GetReplacedTypeOrSelf(dbContextType));
    }
}
