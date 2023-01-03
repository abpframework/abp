using System;
using System.Threading.Tasks;

namespace Volo.Abp.MongoDB;

public interface IMongoDbContextTypeProvider
{
    Type GetDbContextType(Type dbContextType);

    Task<Type> GetDbContextTypeAsync(Type dbContextType);
}
