using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MongoDB;

public class MongoDbContextTypeProvider : IMongoDbContextTypeProvider, ITransientDependency
{
    private readonly AbpMongoDbContextOptions _options;

    public MongoDbContextTypeProvider(IOptions<AbpMongoDbContextOptions> options)
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
