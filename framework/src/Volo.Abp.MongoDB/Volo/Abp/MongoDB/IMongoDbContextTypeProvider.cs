using System;

namespace Volo.Abp.MongoDB;

public interface IMongoDbContextTypeProvider
{
    Type GetDbContextType(Type dbContextType);
}
